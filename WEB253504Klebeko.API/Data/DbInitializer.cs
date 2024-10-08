using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using WEB253504Klebeko.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace WEB253504Klebeko.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();

            await context.Database.MigrateAsync();

            var baseUrl = app.Configuration["ApiBaseUrl"];


            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Жаропонижающие", NormalizedName = "antipyretic" },
                    new Category { Name = "Антибиотики", NormalizedName = "antibiotics" },
                    new Category { Name = "Капли", NormalizedName = "drops" },
                    new Category { Name = "Антигистаминные", NormalizedName = "antihistamines" },
                    new Category { Name = "Сиропы", NormalizedName = "syrups" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
            if (!context.Medicines.Any())
            {
                var categories = await context.Categories.ToListAsync();

                var medicines = new[]
                {
                    new Medicines
                    {
                        Name = "Эффералган",
                        Description = "Жаропонижающее средство.",
                        CategoryId = categories.First(c => c.NormalizedName == "antipyretic").Id,
                        Price = 15,
                        Image = $"{baseUrl}/Images/12.jpg",
                        Mime = "image/jpeg"
                    },
                    new Medicines
                    {
                        Name = "Доксициклин",
                        Description = "Антибиотик широкого спектра действия.",
                        CategoryId = categories.First(c => c.NormalizedName == "antibiotics").Id,
                        Price = 3,
                        Image = $"{baseUrl}/Images/22.jpg",
                        Mime = "image/jpeg"

                    },
                    new Medicines
                    {
                        Name = "Нафазолин",
                        Description = "Назальные капли.",
                        CategoryId = categories.First(c => c.NormalizedName == "drops").Id,
                        Price = 1,
                        Image = $"{baseUrl}/Images/32.jpg",
                        Mime = "image/jpeg"
                    },
                    new Medicines
                    {
                        Name = "Фенкарол",
                        Description = "Антигистаминное средство.",
                        CategoryId = categories.First(c => c.NormalizedName == "antihistamines").Id,
                        Price = 16,
                        Image = $"{baseUrl}/Images/42.jpg",
                        Mime = "image/jpeg"
                    },
                    new Medicines
                    {
                        Name = "Доктор Мом",
                        Description = "Сироп от кашля.",
                        CategoryId = categories.First(c => c.NormalizedName == "syrups").Id,
                        Price = 10,
                        Image = $"{baseUrl}/Images/52.jpg",
                        Mime = "image/jpeg"
                    }
                    };

                    await context.Medicines.AddRangeAsync(medicines);
                    await context.SaveChangesAsync();
            }



        }
    
    }
}
