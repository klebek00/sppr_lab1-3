using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.UI.Services.CategoryService;
using WEB253504Klebeko.UI.Services.FileService;
using WEB253504Klebeko.UI.Services.MedicineService;

namespace WEB253504Klebeko.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {

        private readonly IMedicineService _mediicineService;
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;
        


        public CreateModel(IMedicineService medicineService, ICategoryService categoryService, IFileService fileService)
        {
            _mediicineService = medicineService;
            _categoryService = categoryService;
            _fileService = fileService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var categoryResponse = await _categoryService.GetCategoryListAsync(); // Ожидание асинхронного метода

            if (categoryResponse.Successfull) // Убедитесь, что используется правильное свойство Successful
            {
                var categories = categoryResponse.Data; // Получаем список категорий из ответа
                Categories = new SelectList(categories, "Id", "Name");
            }
            else
            {
                // Обработка ошибки получения категорий, если необходимо
                Categories = new SelectList(Enumerable.Empty<SelectListItem>()); // Пустой список в случае ошибки
            }

            return Page();
        }



        [BindProperty]
        public Medicines Medicines { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public SelectList Categories { get; set; }



        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            

            // Получаем категорию на основе CategoryId
            var categoryResponse = await _categoryService.GetCategoryByIdAsync(Medicines.CategoryId);
            if (!categoryResponse.Successfull || categoryResponse.Data == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Category");
                return Page();
            }

            //// Присваиваем категорию медикаменту
            Medicines.Category = categoryResponse.Data;

            // Если было загружено изображение
            if (Image != null)
            {
                try
                {

                    Medicines.Image = Image.FileName; // Сохраняем URL изображения
                    Medicines.Mime = Image.ContentType; // Сохраняем Mime тип
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error uploading image: " + ex.Message);
                    return Page();
                }
            }

            //if (!ModelState.IsValid)
            //{
            //    foreach (var modelState in ModelState.Values)
            //    {
            //        foreach (var error in modelState.Errors)
            //        {
            //            Console.WriteLine(error.ErrorMessage);
            //        }
            //    }
            //    return Page();
            //}

            // Создаем медикамент через сервис
            await _mediicineService.CreateMedicAsync(Medicines, Image);

            return RedirectToPage("./Index");
        }
    }
}
