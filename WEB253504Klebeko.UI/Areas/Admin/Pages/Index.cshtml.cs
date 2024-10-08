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
    public class IndexModel : PageModel
    {
        private readonly IMedicineService _mediicineService;

        public IndexModel(IMedicineService medicineService)
        {
            _mediicineService = medicineService;
        }

        public IList<Medicines> Medicines { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var medicines = await _mediicineService.GetMedicListAsync(pageSize: 1000);
            Medicines = medicines.Data.Items;

        }
    }
}
