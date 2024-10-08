using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.UI.Services.MedicineService;

namespace WEB253504Klebeko.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IMedicineService _mediicineService;

        public DetailsModel(IMedicineService medicineService)
        {
            _mediicineService = medicineService;
        }

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
    }
}
