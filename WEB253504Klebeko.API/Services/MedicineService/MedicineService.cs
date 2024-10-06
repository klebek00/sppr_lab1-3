using WEB253504Klebeko.Domain.Models;
using WEB253504Klebeko.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WEB253504Klebeko.API.Data;
using System;

namespace WEB253504Klebeko.API.Services.MedicineService
{
    public class MedicineService : IMedicineService
    {
        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _context;
        private readonly string _imagePath;

        public MedicineService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ResponseData<ListModel<Medicines>>> GetMedicListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Medicines.AsQueryable();
            var dataList = new ListModel<Medicines>();

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                query = query.Where(d => d.Category.ToLower() == categoryNormalizedName.ToLower());
            }

            var count = await query.CountAsync();
            if (count == 0)
            {
                return ResponseData<ListModel<Medicines>>.Success(dataList);
            }

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            if (pageNo > totalPages)
                return ResponseData<ListModel<Medicines>>.Error("No such page");

            dataList.Items = await query
                .OrderBy(d => d.Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return ResponseData<ListModel<Medicines>>.Success(dataList);
        }


        public async Task<ResponseData<Medicines>> CreateMedicAsync(Medicines medicine)
        {
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();
            return ResponseData<Medicines>.Success(medicine);
        }

        public async Task DeleteMedicAsync(int id)
        {
            var medDel = _context.Medicines.FirstOrDefault(d => d.Id == id);
            if (medDel == null)
                throw new KeyNotFoundException("Medicine not found");
            _context.Remove(medDel);
            await _context.SaveChangesAsync();  
        }

        public async Task<ResponseData<Medicines>> GetMedicByIdAsync(int id)
        {
            var med = await _context.Medicines.Include(m => m.Category).FirstOrDefaultAsync(m => m.Id == id);
            if (med == null)
                return ResponseData<Medicines>.Error("Medicine not found");

            return ResponseData<Medicines>.Success(med);
        }

        public async Task UpdateMedicAsync(int id, Medicines med)
        {
            var medToUpdate = _context.Medicines.Find(id) ?? throw new KeyNotFoundException();

            medToUpdate.Price = med.Price;
            medToUpdate.Description = med.Description;
            medToUpdate.Name = med.Name;
            medToUpdate.CategoryId = med.CategoryId;

            _context.Update(medToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

    }
}
