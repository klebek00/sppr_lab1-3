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
    public class IndexModel : PageModel
    {
        private readonly WEB253504Klebeko.API.Data.AppDbContext _context;

        public IndexModel(WEB253504Klebeko.API.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Medicines> Medicines { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Medicines = await _context.Medicines.ToListAsync();
        }
    }
}
