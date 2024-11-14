using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.API.Services.MedicineService;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;
using WEB253504Klebeko.UI.Services.MedicineService;

namespace WEB253504Klebeko.Tests
{
    public class MedicineServiceTests
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly MedicineService _medicineService;

        public MedicineServiceTests()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            using (var context = new AppDbContext(_contextOptions))
            {
                context.Database.EnsureCreated();

                var category1 = new Category { Name = "Жаропонижающие", NormalizedName = "antipyretic" };
                var category2 = new Category { Name = "Антибиотики", NormalizedName = "antibiotics" };
                context.Categories.AddRange(category1, category2);
                context.SaveChanges();

                var medicines = new List<Medicines>
            {
                new Medicines { Name = "Эффералган",
                                Description = "Жаропонижающее средство.",
                                CategoryId = 1,
                                Price = 15,
                                Image = $"/Images/12.jpg",
                                Mime = "image/jpeg" },

                new Medicines { Name = "Доксициклин",
                                Description = "Антибиотик широкого спектра действия.",
                                CategoryId = 2,
                                Price = 3,
                                Image = $"/Images/22.jpg",
                                Mime = "image/jpeg" },

                new Medicines {  Name = "Нафазолин",
                                Description = "Назальные капли.",
                                CategoryId = 1,
                                Price = 1,
                                Image = $"/Images/32.jpg",
                                Mime = "image/jpeg" },

                new Medicines { Name = "Фенкарол",
                                Description = "Антигистаминное средство.",
                                CategoryId = 2,
                                Price = 16,
                                Image = $"/Images/42.jpg",
                                Mime = "image/jpeg" },
                new Medicines { Name = "Фенкарол",
                                Description = "Антигистаминное средство.",
                                CategoryId = 2,
                                Price = 16,
                                Image = $"/Images/42.jpg",
                                Mime = "image/jpeg" },
                new Medicines { Name = "Фенкарол",
                                Description = "Антигистаминное средство.",
                                CategoryId = 2,
                                Price = 16,
                                Image = $"/Images/42.jpg",
                                Mime = "image/jpeg" }
            };
                context.Medicines.AddRange(medicines);
                context.SaveChanges();
            }

        }
        AppDbContext CreateContext() => new AppDbContext(_contextOptions);
        public void Dispose() => _connection.Dispose();

        [Fact]
        public async Task ServiceReturnsFirstPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new MedicineService(context, Substitute.For<ILogger<MedicineService>>());
            var result = await service.GetMedicListAsync(null, 1, 3);

            Assert.IsType<ResponseData<ListModel<Medicines>>>(result);
            Assert.True(result.Successfull);  
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal(context.Medicines.First(), result.Data.Items[0]);
        }

        [Fact]
        public void GetMedicineListAsync_WithPageNumber_ReturnsRightPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new MedicineService(context, Substitute.For<ILogger<MedicineService>>());
            var result = service.GetMedicListAsync(null, pageNo: 2).Result;
            Assert.True(result.Successfull);
            Assert.IsType<ResponseData<ListModel<Medicines>>>(result);
            Assert.Equal(2, result.Data?.CurrentPage);
            Assert.Equal(3, result.Data?.Items.Count);
            Assert.Equal(2, result.Data?.TotalPages);
            var expectedItems = context.Medicines.AsEnumerable().Skip(3).Take(3).ToList();
            Assert.Equal(expectedItems.Count, result.Data?.Items.Count);
            for (int i = 0; i < expectedItems.Count; i++)
            {
                Assert.Equal(expectedItems[i].Id, result.Data?.Items[i].Id);
                Assert.Equal(expectedItems[i].Name, result.Data?.Items[i].Name);
                Assert.Equal(expectedItems[i].Description, result.Data?.Items[i].Description);
                Assert.Equal(expectedItems[i].CategoryId, result.Data?.Items[i].CategoryId);
            }
        }

        [Fact]
        public void GetMedicineListAsync_WithCategoryFilter_ReturnsFilteredByCategory()
        {
            using var context = CreateContext();
            var service = new MedicineService(context, Substitute.For<ILogger<MedicineService>>());
            var result = service.GetMedicListAsync("antipyretic").Result;
            Assert.True(result.Successfull);
            Assert.IsType<ResponseData<ListModel<Medicines>>>(result);
            Assert.Equal(1, result.Data?.CurrentPage);
            Assert.Equal(2, result.Data?.Items.Count);
            Assert.Equal(1, result.Data?.TotalPages);
            Assert.Equal(context.Medicines.AsEnumerable().Where((b) => b.CategoryId == 1).Take(3), result.Data?.Items);
        }
        [Fact]
        public void GetMedicineListAsync_MaxSizeSucceded_ReturnsMaximumMaxSize()
        {
            using var context = CreateContext();
            var service = new MedicineService(context, Substitute.For<ILogger<MedicineService>>());
            var result = service.GetMedicListAsync(null, pageSize: 30).Result;
            Assert.True(result.Successfull);
            Assert.IsType<ResponseData<ListModel<Medicines>>>(result);
            Assert.Equal(1, result.Data?.CurrentPage);
            Assert.Equal(6, result.Data?.Items.Count);
            Assert.Equal(1, result.Data?.TotalPages);
        }

        [Fact]
        public void GetMedicineListAsync_PageNoIncorrect_ReturnsError()
        {
            using var context = CreateContext();
            var service = new MedicineService(context, Substitute.For<ILogger<MedicineService>>());
            var result = service.GetMedicListAsync(null, pageNo: 100).Result;
            Assert.False(result.Successfull);
        }
    }
}
