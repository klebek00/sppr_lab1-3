using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB253504Klebeko.UI.Extensions;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.UI.Models;
using WEB253504Klebeko.API.Services.MedicineService;
using WEB253504Klebeko.UI.Services.MedicineService;
using WEB253504Klebeko.UI.Services.CategoryService;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.RegisterCustomServices();

builder.Services.AddRazorPages();

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>()!;

builder.Services
.AddHttpClient<WEB253504Klebeko.UI.Services.MedicineService.IMedicineService, ApiMedicineService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services
.AddHttpClient<WEB253504Klebeko.UI.Services.CategoryService.ICategoryService, ApiCategoryService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
