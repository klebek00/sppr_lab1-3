using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using WEB253504Klebeko.API.Data;
using WEB253504Klebeko.API.Models;
using WEB253504Klebeko.API.Services.CategoryService;
using WEB253504Klebeko.API.Services.MedicineService;

var builder = WebApplication.CreateBuilder(args);

string connectionDBString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionDBString));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var authServer = builder.Configuration
    .GetSection("AuthServer")
    .Get<AuthServerData>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
{
    // Адрес метаданных конфигурации OpenID
    o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
    // Authority сервера аутентификации
    o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
    // Audience для токена JWT
    o.Audience = "account";
    // Запретить HTTPS для использования локальной версии Keycloak
    // В рабочем проекте должно быть true
    o.RequireHttpsMetadata = false;
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

var app = builder.Build();

await DbInitializer.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
