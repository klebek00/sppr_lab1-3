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

namespace WEB253504Klebeko.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IMedicineService _mediicineService;

        public EditModel(IMedicineService medicineService)
        {
            _mediicineService = medicineService;
        }

        [BindProperty]
        public Medicines Medicines { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }



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
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mediicineService.UpdateMedicAsync(Medicines.Id, Medicines, Image);

            return RedirectToPage("./Index");
        }

    }
}
