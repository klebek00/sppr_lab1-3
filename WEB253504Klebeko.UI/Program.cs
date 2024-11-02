using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB253504Klebeko.UI.Extensions;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.UI.Models;
using WEB253504Klebeko.API.Services.MedicineService;
using WEB253504Klebeko.UI.Services.MedicineService;
using WEB253504Klebeko.UI.Services.CategoryService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Configuration;
using WEB253504Klebeko.UI.HelperClasses;
using WEB253504Klebeko.UI.Authorization;
using WEB253504Klebeko.Domain.Models;
using WEB253504Klebeko.UI.Services.CartService;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.RegisterCustomServices();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IAuthService, KeycloakAuthService>();


var uriData = builder.Configuration.GetSection("UriData").Get<UriData>()!;

builder.Services
.AddHttpClient<WEB253504Klebeko.UI.Services.MedicineService.IMedicineService, ApiMedicineService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services
.AddHttpClient<WEB253504Klebeko.UI.Services.CategoryService.ICategoryService, ApiCategoryService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddScoped<Cart>();

builder.Services.AddHttpContextAccessor();

var keycloakData =
builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer()
.AddOpenIdConnect(options =>
{
    options.Authority =
    $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
    options.ClientId = keycloakData.ClientId;
    options.ClientSecret = keycloakData.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("openid");
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false;
    options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
});

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
