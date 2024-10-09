using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.UI.Services.FileService;
using WEB253504Klebeko.UI.Services.MedicineService;

namespace WEB253504Klebeko.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IMedicineService _medicineService;
        private readonly IFileService _fileService;

        public DetailsModel(IMedicineService medicineService, IFileService fileService)
        {
            _medicineService = medicineService;
            _fileService = fileService;
        }

        [BindProperty]
        public Medicines Medicines { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _medicineService.GetMedicByIdAsync((int)id);
            if (medicine == null)
            {
                return NotFound();
            }

            Medicines = medicine.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _medicineService.DeleteMedicAsync(id);
            await _fileService.DeleteFileAsync(Medicines.Image);

            return RedirectToPage("./Index");
        }
    }
}
