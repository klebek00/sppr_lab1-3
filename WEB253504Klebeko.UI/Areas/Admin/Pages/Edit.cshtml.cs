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

namespace WEB253504Klebeko.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly WEB253504Klebeko.API.Data.AppDbContext _context;

        public EditModel(WEB253504Klebeko.API.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Medicines Medicines { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicines =  await _context.Medicines.FirstOrDefaultAsync(m => m.Id == id);
            if (medicines == null)
            {
                return NotFound();
            }
            Medicines = medicines;
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

            _context.Attach(Medicines).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicinesExists(Medicines.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MedicinesExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}
