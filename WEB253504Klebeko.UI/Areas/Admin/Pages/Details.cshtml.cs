using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.Domain.Entities;

namespace WEB253504Klebeko.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly WEB253504Klebeko.API.Data.AppDbContext _context;

        public DetailsModel(WEB253504Klebeko.API.Data.AppDbContext context)
        {
            _context = context;
        }

        public Medicines Medicines { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicines = await _context.Medicines.FirstOrDefaultAsync(m => m.Id == id);
            if (medicines == null)
            {
                return NotFound();
            }
            else
            {
                Medicines = medicines;
            }
            return Page();
        }
    }
}
