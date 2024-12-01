using WEB253504Klebeko.Domain.Entities;
namespace WEB253504Klebeko.BlazorWasm.Services
{
    public interface IDataService
    {
        event Action DataLoaded;

        List<Category> Categories { get; set; }

        List<Medicines> Medicines { get; set; }

        bool Success { get; set; }

        string ErrorMessage { get; set; }

        int TotalPages { get; set; }

        int CurrentPage { get; set; }

        Category? SelectedCategory { get; set; }

        Task GetProductListAsync(int pageNo = 1);

        Task GetCategoryListAsync();
    }
}
