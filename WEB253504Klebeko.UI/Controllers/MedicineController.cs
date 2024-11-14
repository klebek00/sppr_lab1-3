using Microsoft.AspNetCore.Mvc;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.UI.Extensions;
using WEB253504Klebeko.UI.Services.CategoryService;
using WEB253504Klebeko.UI.Services.MedicineService;

namespace WEB253504Klebeko.UI.Controllers
{
    [Route("Catalog")]
    public class MedicineController : Controller
	{
		private readonly IMedicineService _medicineService;
		private readonly ICategoryService _categoryService;

		public MedicineController(IMedicineService productService, ICategoryService categoryService)
		{
			_medicineService = productService;
			_categoryService = categoryService;
		}
        [HttpGet("")]
        [HttpGet("{category}")]
        [HttpGet("{category?}/Page_{pageNo:int}")]
        public async Task<IActionResult> Index(string? category, int pageNo = 1)
		{
			var medicResponse = await _medicineService.GetMedicListAsync(category, pageNo);
			if (!medicResponse.Successfull)
				return NotFound(medicResponse.ErrorMessage);

            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Successfull)
                return NotFound(categoriesResponse.ErrorMessage);

            var currentCategory = categoriesResponse.Data?
                            .FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Всe";

            ViewBag.Categories = categoriesResponse.Data;
            ViewBag.CurrentCategory = currentCategory;


            if (Request.IsAjaxRequest()) 
            {
                return PartialView("_MedicineListPartial", medicResponse.Data);
            }

            ViewData["categories"] = categoriesResponse.Data;
            ViewData["currentCategory"] = currentCategory;
            return View(medicResponse.Data);
		}
	}
}
