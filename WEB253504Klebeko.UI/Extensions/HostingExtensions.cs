using WEB253504Klebeko.UI.Models;
using WEB253504Klebeko.UI.Services.CategoryService;
using WEB253504Klebeko.UI.Services.FileService;
using WEB253504Klebeko.UI.Services.MedicineService;

namespace WEB253504Klebeko.UI.Extensions
{
	public static class HostingExtensions
	{
		public static void RegisterCustomServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<ICategoryService, MemoryCetegoryService>();
			builder.Services.AddScoped<IMedicineService, MemoryMedicinesService>();
            var uriData = builder.Configuration.GetSection("UriData").Get<UriData>()!;
            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt => opt.BaseAddress = new Uri($"{uriData.ApiUri}Files"));
        }
	}
}
