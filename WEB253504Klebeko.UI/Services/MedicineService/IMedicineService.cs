using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;

namespace WEB253504Klebeko.UI.Services.MedicineService
{
	public interface IMedicineService
	{
		public Task<ResponseData<ListModel<Medicines>>> GetMedicListAsync(string? categoryNormalizedName = null, int pageNo = 1, int pageSize = -1);
		public Task<ResponseData<Medicines>> GetMedicByIdAsync(int id);
		public Task UpdateMedicAsync(int id, Medicines product, IFormFile? formFile);
		public Task DeleteMedicAsync(int id);
		public Task<ResponseData<Medicines>> CreateMedicAsync(Medicines product, IFormFile?formFile);
	}
}
