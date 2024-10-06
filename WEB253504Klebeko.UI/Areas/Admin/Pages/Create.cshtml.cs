using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.Domain.Entities;

namespace WEB253504Klebeko.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly WEB253504Klebeko.API.Data.AppDbContext _context;


        public CreateModel(WEB253504Klebeko.API.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Medicines Medicines { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Medicines.Add(Medicines);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
