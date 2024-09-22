using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using WEB253504Klebeko.Domain.Entities;

namespace WEB253504Klebeko.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Medicines> Medicines { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
