using Microsoft.EntityFrameworkCore;
using Movie.Models;
using System.Reflection;

namespace Movie.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}

        public DbSet<Category> Categories { get; set; }
        public DbSet<Film> Films { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
