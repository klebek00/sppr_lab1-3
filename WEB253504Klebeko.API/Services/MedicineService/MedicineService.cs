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

        private readonly ILogger _logger;


        public MedicineService(AppDbContext context, ILogger<MedicineService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<ResponseData<ListModel<Medicines>>> GetMedicListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Medicines
                .Include(m => m.Category) // Подгружаем связанные категории
                .AsQueryable();

            var dataList = new ListModel<Medicines>();

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                // Фильтруем по нормализованному имени категории
                query = query.Where(d => d.Category.NormalizedName.ToLower() == categoryNormalizedName.ToLower());
            }

            var count = await query.CountAsync();
            if (count == 0)
            {
                return ResponseData<ListModel<Medicines>>.Success(dataList);
            }

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            if (pageNo > totalPages)
                return ResponseData<ListModel<Medicines>>.Error("No such page");

            // Получаем список медикаментов с учетом фильтров и пагинации
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
            // Убедитесь, что Id медикамента не установлен вручную (если он генерируется автоматически)
            medicine.Id = 0; // Сбрасываем Id, если необходимо

            _logger.LogInformation($"Создание медикамента: {medicine.Name}, CategoryId: {medicine.CategoryId}");

            // Убедитесь, что категория не создается заново, если она уже существует
            if (medicine.Category != null)
            {
                // Присоединяем категорию к контексту, чтобы EF не пытался добавить её как новую
                _context.Categories.Attach(medicine.Category);
            }

            // Добавляем медикамент в контекст
            _context.Medicines.Add(medicine);

            try
            {
                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении данных.");
                return ResponseData<Medicines>.Error("Ошибка при сохранении данных.");
            }

            // Возвращаем успешный ответ с созданным медикаментом
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
            medToUpdate.Image = med.Image;

            _context.Update(medToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

    }
}
