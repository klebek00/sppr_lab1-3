using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.UI.Services.MedicineService;
using WEB253504Klebeko.UI.Services.CategoryService;
using WEB253504Klebeko.UI.Services.FileService;

namespace WEB253504Klebeko.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IMedicineService _mediicineService;
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;



        public EditModel(IMedicineService medicineService, ICategoryService categoryService, IFileService fileService)
        {
            _mediicineService = medicineService;
            _categoryService = categoryService;
            _fileService = fileService;

        }

        [BindProperty]
        public Medicines Medicines { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }
        public SelectList Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicines =  await _mediicineService.GetMedicByIdAsync((int)id);
            if (medicines == null)
            {
                return NotFound();
            }
            Medicines = medicines.Data;

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var categoryResponse = await _categoryService.GetCategoryByIdAsync(Medicines.CategoryId);
            if (!categoryResponse.Successfull || categoryResponse.Data == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Category");
                return Page();
            }

            Medicines.Category = categoryResponse.Data;

            var existingMedicineResponse = await _mediicineService.GetMedicByIdAsync(Medicines.Id);
            if (!existingMedicineResponse.Successfull || existingMedicineResponse.Data == null)
            {
                return NotFound();
            }

            var existingMedicine = existingMedicineResponse.Data;

            if (Image == null)
            {
                Medicines.Image = existingMedicine.Image;
            }
            else
            {
                Medicines.Image = Image.FileName;

            }
            await _fileService.DeleteFileAsync(existingMedicine.Image);
            Medicines.Mime = Image.ContentType;

            await _mediicineService.UpdateMedicAsync(Medicines.Id, Medicines, Image);

            return RedirectToPage("./Index");
        }

    }
}
