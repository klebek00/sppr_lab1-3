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


            if (context.Medicines.Any() && context.Categories.Any())
            {
                return;
            }

            var antipyretic = new Category { Name = "Жаропонижающие", NormalizedName = "antipyretic" };
            var antibiotics = new Category { Name = "Антибиотики", NormalizedName = "antibiotics" };
            var drops = new Category { Name = "Капли", NormalizedName = "drops" };
            var antihistamines = new Category { Name = "Антигистаминные", NormalizedName = "antihistamines" };
            var syrups = new Category { Name = "Сиропы", NormalizedName = "syrups" };

            var categories = new List<Category>() { antipyretic, antibiotics, drops, antihistamines, syrups };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var medicines = new[]
            {
                new Medicines
                {
                    Name = "Эффералган",
                    Description = "Жаропонижающее средство.",
                    CategoryId = antipyretic.Id,
                    Price = 15,
                    Image = $"{baseUrl}/Images/12.jpg",
                    Category = antipyretic.NormalizedName,
                    Mime = "image/jpeg" 
                },
                new Medicines 
                { 
                    Name = "Доксициклин", 
                    Description = "Антибиотик широкого спектра действия.", 
                    CategoryId = antibiotics.Id,
                    Category = antibiotics.NormalizedName,
                    Price = 3, 
                    Image = $"{baseUrl}/Images/22.jpg", 
                    Mime = "image/jpeg" 
                    
                },
                new Medicines 
                { 
                    Name = "Нафазолин", 
                    Description = "Назальные капли.", 
                    CategoryId = drops.Id, 
                    Category = drops.NormalizedName,
                    Price = 1, 
                    Image = $"{baseUrl}/Images/32.jpg", 
                    Mime = "image/jpeg" 
                },
                new Medicines 
                { 
                    Name = "Фенкарол", 
                    Description = "Антигистаминное средство.", 
                    CategoryId = antihistamines.Id, 
                    Category = antihistamines.NormalizedName,
                    Price = 16, 
                    Image = $"{baseUrl}/Images/42.jpg", 
                    Mime = "image/jpeg" 
                },
                new Medicines 
                { 
                    Name = "Доктор Мом", 
                    Description = "Сироп от кашля.", 
                    CategoryId = syrups.Id, 
                    Category = syrups.NormalizedName,
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
