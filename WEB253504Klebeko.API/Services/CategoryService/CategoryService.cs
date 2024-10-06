using NuGet.Protocol;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;

namespace WEB253504Klebeko.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {

        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var category = _context.Categories.ToList();
            var result = ResponseData<List<Category>>.Success(category);
            return Task.FromResult(result);
        }
    }
}
