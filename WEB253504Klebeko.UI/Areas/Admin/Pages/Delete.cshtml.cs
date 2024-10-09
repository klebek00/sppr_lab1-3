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
    public class DeleteModel : PageModel
    {
        private readonly IMedicineService _mediicineService;
        private readonly IFileService _fileService;

        public DeleteModel(IMedicineService medicineService, IFileService fileService)
        {
            _mediicineService = medicineService;
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

            var medicines = await _mediicineService.GetMedicByIdAsync((int)id);

            if (medicines == null)
            {
                return NotFound();
            }
            else
            {
                Medicines = medicines.Data;
            }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var medicines = await _mediicineService.GetMedicByIdAsync((int)id);

            if (medicines.Data != null)
            {
                string file = medicines.Data.Image;

                Medicines = medicines.Data;
                await _mediicineService.DeleteMedicAsync(medicines.Data.Id);
                await _fileService.DeleteFileAsync(file);

            }

            return RedirectToPage("./Index");
        }
    }
}
