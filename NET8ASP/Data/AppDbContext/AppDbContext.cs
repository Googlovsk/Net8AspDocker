using Microsoft.EntityFrameworkCore;
using NET8ASP.Models.Domain;

namespace NET8ASP.Data.AppDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "A" },
                new Category { Id = 2, Name = "B" },
                new Category { Id = 3, Name = "C" }
                );
        }
    }
}
