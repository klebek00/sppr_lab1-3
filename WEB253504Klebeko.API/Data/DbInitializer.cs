using Microsoft.EntityFrameworkCore;
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


            if (context.Medicines.Any() && context.Categories.Any())
            {
                return;
            }

            var categories = new[]
            {
                new Category { Name = "Жаропонижающие", NormalizedName = "antipyretic" },
                new Category { Name = "Антибиотики", NormalizedName = "antibiotics" },
                new Category { Name = "Капли", NormalizedName = "drops" },
                new Category { Name = "Антигистаминные", NormalizedName = "antihistamines" },
                new Category { Name = "Сиропы", NormalizedName = "syrups" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var medicines = new[]
            {
                new Medicines { Name = "Эффералган", Description = "Жаропонижающее средство.", Category = categories.First(c => c.NormalizedName == "antipyretic"), Price = 15, Image = $"{baseUrl}/Images/12.jpg", Mime = "image/jpeg" },
                new Medicines { Name = "Доксициклин", Description = "Антибиотик широкого спектра действия.", Category = categories.First(c => c.NormalizedName == "antibiotics"), Price = 3, Image = $"{baseUrl}/Images/22.jpg", Mime = "image/jpeg" },
                new Medicines { Name = "Нафазолин", Description = "Назальные капли.", Category = categories.First(c => c.NormalizedName == "drops"), Price = 1, Image = $"{baseUrl}/Images/32.jpg", Mime = "image/jpeg" },
                new Medicines { Name = "Фенкарол", Description = "Антигистаминное средство.", Category = categories.First(c => c.NormalizedName == "antihistamines"), Price = 16, Image = $"{baseUrl}/Images/42.jpg", Mime = "image/jpeg" },
                new Medicines { Name = "Доктор Мом", Description = "Сироп от кашля.", Category = categories.First(c => c.NormalizedName == "syrups"), Price = 10, Image = $"{baseUrl}/Images/52.jpg", Mime = "image/jpeg" }
            };

            await context.Medicines.AddRangeAsync(medicines);
            await context.SaveChangesAsync();
        }
    
    }
}
