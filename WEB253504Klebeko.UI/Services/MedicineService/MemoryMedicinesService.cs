using Microsoft.AspNetCore.Mvc;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;
using WEB253504Klebeko.UI.Services.CategoryService;

namespace WEB253504Klebeko.UI.Services.MedicineService
{
    public class MemoryMedicinesService : IMedicineService
    {
        List<Medicines> _medicines;
        List<Category> _categories;
        private readonly IConfiguration _configuration;

        public MemoryMedicinesService(IConfiguration config, ICategoryService categoryService)
        {
            _configuration = config;
            _categories = categoryService.GetCategoryListAsync()
                                        .Result
                                        .Data;
            SetupData();
        }

        private void SetupData()
        {
            _medicines = new List<Medicines>
            {
                new Medicines { Id = 1, Name = "Эффералган", Description = "Description for Product 1", Category = _categories.Find(c=>c.NormalizedName.Equals("antipyretic")), Price = 15, Image = "/images/12.jpg", Mime = "image/jpeg" },
                new Medicines { Id = 2, Name = "Доксициклин", Description = "Description for Product 2", Category = _categories.Find(c=>c.NormalizedName.Equals("antibiotics")), Price = 3, Image = "/images/22.jpg", Mime = "image/jpeg" },
                new Medicines { Id = 3, Name = "Нафазолин", Description = "Description for Product 2", Category = _categories.Find(c=>c.NormalizedName.Equals("drops")), Price = 1, Image = "/images/32.jpg", Mime = "image/jpeg" },
                new Medicines { Id = 4, Name = "Фенкарол", Description = "Description for Product 2", Category = _categories.Find(c=>c.NormalizedName.Equals("antihistamines")), Price = 16, Image = "/images/42.jpg", Mime = "image/jpeg" },
                new Medicines { Id = 5, Name = "Доктор Мом", Description = "Description for Product 3", Category = _categories.Find(c=>c.NormalizedName.Equals("syrups")), Price = 10, Image = "/images/52.jpg", Mime = "image/jpeg" }
            };
        }


        public async Task<ResponseData<ListModel<Medicines>>> GetMedicListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            int itemsPerPage = _configuration.GetValue<int>("ItemsPerPage");

            var data = _medicines
                .Where(d => categoryNormalizedName == null ||
                    d.Category.NormalizedName.Equals(categoryNormalizedName))
                .ToList();

            int totalItems = data.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            var productsForPage = data
                .Skip((pageNo - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            var result = new ListModel<Medicines>
            {
                CurrentPage = pageNo,
                TotalPages = totalPages,
                Items = productsForPage
            };


            return ResponseData<ListModel<Medicines>>.Success(result);
        }

        public Task<ResponseData<Medicines>> CreateMedicAsync(Medicines product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMedicAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Medicines>> GetMedicByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMedicAsync(int id, Medicines product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

    }
}
