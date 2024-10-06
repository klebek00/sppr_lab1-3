using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;

namespace WEB253504Klebeko.API.Services.MedicineService
{
    public interface IMedicineService
    {
        public Task<ResponseData<ListModel<Medicines>>> GetMedicListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        public Task<ResponseData<Medicines>> GetMedicByIdAsync(int id);
        public Task UpdateMedicAsync(int id, Medicines product);
        public Task DeleteMedicAsync(int id);
        public Task<ResponseData<Medicines>> CreateMedicAsync(Medicines product);

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }
}
