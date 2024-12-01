using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;
using WEB253504Klebeko.API.Services.MedicineService;
using Azure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WEB253504Klebeko.API.Controllers
{
    //[Authorize(Policy = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMedicineService _medicineService;

        public MedicinesController(AppDbContext context, IMedicineService medicineService)
        {
            _context = context;
            _medicineService = medicineService;
        }


        [HttpGet("category/{categoryNormalizedName}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<List<Medicines>>>> GetMedicinesByCategory(string categoryNormalizedName, [FromQuery] int pageNo = 1, [FromQuery] int pageSize = 3)
        {
            Console.WriteLine($"Category: {categoryNormalizedName}, Page No: {pageNo}, Page Size: {pageSize}");
            return Ok(await _medicineService.GetMedicListAsync(categoryNormalizedName, pageNo, pageSize));
        }

        [HttpGet("page{pageNo}")]
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Medicines>>>> GetMedicines(string? category, int pageNo = 1, int pageSize = 3)
        {
            Console.WriteLine($"Category: {category}, Page No: {pageNo}, Page Size: {pageSize}");
            return Ok(await _medicineService.GetMedicListAsync(
                category,
                pageNo,
                pageSize));
        }


        // GET: api/Medicines/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<Medicines>>> GetMedicine(int id)
        {
            var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
            Console.WriteLine($"User Roles AAAAAAAAAAA: {string.Join(", ", roles)}");
            var userClaims = User.Claims.ToList(); // Получаем все claims для отладки
            Console.WriteLine("User Claims:");
            foreach (var claim in userClaims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            var medResponse = await _medicineService.GetMedicByIdAsync(id);
            return Ok(medResponse);
        }

        // PUT: api/Medicines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> PutMedicines(int id, Medicines medicines)
        {
            
            await _medicineService.UpdateMedicAsync(id, medicines);
            return NoContent();
        }

        // POST: api/Medicines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<Medicines>>> PostMedicines(Medicines medicines)
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
            Console.WriteLine($"User Roles: {string.Join(", ", roles)}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Вернуть 400, если модель не валидна
            }
            var createMed = await _medicineService.CreateMedicAsync(medicines);

            if (createMed == null || createMed.Data == null)
            {
                return StatusCode(500, "Ошибка при создании медикамента.");
            }

            return CreatedAtAction("GetMedicines", new { id = createMed.Data.Id }, createMed);
        }



        // DELETE: api/Medicines/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteMedicines(int id)
        {
            var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

            Console.WriteLine($"User Roles: {string.Join(", ", roles)}");


            await _medicineService.DeleteMedicAsync(id);
            return NoContent();
        }

        private bool MedicinesExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}
