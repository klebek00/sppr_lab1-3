using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;

namespace WEB253504Klebeko.UI.Services.CategoryService
{
	public class MemoryCetegoryService : ICategoryService
	{
        public Task<ResponseData<Category>> GetCategoryByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
		{
			var categories = new List<Category>
			{
				new Category {Id=1, Name="Жаропонижающие", NormalizedName="antipyretic"},
				new Category {Id=2, Name="Антибиотики", NormalizedName="antibiotics"},
				new Category {Id=3, Name="Антигистаминные", NormalizedName="antihistamines"},
				new Category {Id=4, Name="Капли", NormalizedName="drops"},
				new Category {Id=5, Name="Сиропы", NormalizedName="syrups"},
			};
			var result = ResponseData<List<Category>>.Success(categories);
			return Task.FromResult(result);
		}
	}
}
