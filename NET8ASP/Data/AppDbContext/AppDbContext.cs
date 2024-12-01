using Microsoft.EntityFrameworkCore;
using NET8ASP.Models.Domain;
using NET8ASP.Models.Domain.Cart;
using NET8ASP.Models.Domain.Order;
using System.Reflection.Emit;

namespace NET8ASP.Data.AppDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Удилища", ParentCategoryId = null },
                new Category { Id = 2, Name = "Катушки", ParentCategoryId = null },
                new Category { Id = 3, Name = "Снасти/Оснастка", ParentCategoryId = null },
                new Category { Id = 4, Name = "Зимние снасти", ParentCategoryId = null },
                new Category { Id = 5, Name = "Прикормки/Насадки", ParentCategoryId = null },
                new Category { Id = 6, Name = "Живая приманка", ParentCategoryId = null },
                new Category { Id = 7, Name = "Экипировка", ParentCategoryId = null },
                new Category { Id = 8, Name = "Инвентарь", ParentCategoryId = null },
                new Category { Id = 9, Name = "Электроника", ParentCategoryId = null },

                new Category { Id = 10, Name = "Спининги", ParentCategoryId = 1 },
                new Category { Id = 11, Name = "Фидера", ParentCategoryId = 1 },
                new Category { Id = 12, Name = "Зимние удилища", ParentCategoryId = 1 },

                new Category { Id = 13, Name = "Леска", ParentCategoryId = 3 },
                new Category { Id = 14, Name = "Крючки", ParentCategoryId = 3 },
                new Category { Id = 15, Name = "Карабины, Вертлюги", ParentCategoryId = 3 },
                new Category { Id = 16, Name = "Груза", ParentCategoryId = 3 },

                new Category { Id = 17, Name = "Блесны", ParentCategoryId = 4 },
                new Category { Id = 18, Name = "Воблера", ParentCategoryId = 4 },

                new Category { Id = 19, Name = "Зимняя одежда", ParentCategoryId = 7 },
                new Category { Id = 20, Name = "Летняя ожеджа", ParentCategoryId = 7 },
                new Category { Id = 21, Name = "Межсезонная одежда", ParentCategoryId = 7 },
                new Category { Id = 22, Name = "Термобелье", ParentCategoryId = 7 },
                new Category { Id = 23, Name = "Перчатки", ParentCategoryId = 7 },
                new Category { Id = 24, Name = "Обувь", ParentCategoryId = 7 },

                new Category { Id = 25, Name = "Рыболовные рюкзаки", ParentCategoryId = 8 },
                new Category { Id = 26, Name = "Термосы", ParentCategoryId = 8 },
                new Category { Id = 27, Name = "Палатки", ParentCategoryId = 8 },
                new Category { Id = 28, Name = "Спальные мешки", ParentCategoryId = 8 },
                new Category { Id = 29, Name = "Фонарики", ParentCategoryId = 8 },
                new Category { Id = 31, Name = "Кресла/Стулья", ParentCategoryId = 8 },
                new Category { Id = 32, Name = "Изотермические контейнеры", ParentCategoryId = 8 },
                new Category { Id = 33, Name = "Для костра", ParentCategoryId = 8 },


                new Category { Id = 34, Name = "Эхолоты", ParentCategoryId = 9 },
                new Category { Id = 35, Name = "Навигаторы", ParentCategoryId = 9 },
                new Category { Id = 36, Name = "Подводные камеры", ParentCategoryId = 9 }
                );
            builder.Entity<Manufacturer>().HasData(
                new Manufacturer { Id = 1, Name = "TestManufacturer_A" },
                new Manufacturer { Id = 2, Name = "TestManufacturer_B" },
                new Manufacturer { Id = 3, Name = "TestManufacturer_C" }
                );
            builder.Entity<Role>().HasData(
               new Role { Id = 1, Name = "Admin" },
               new Role { Id = 2, Name = "User" }
               );
            builder.Entity<User>().HasData(
               new User { Id = 1, Fio = "Admin", Login = "admin", Password = "admin", Phone = "admin", RoleId = 1 },
               new User { Id = 2, Fio = "Иванов Иван Иванович", Login = "ivan", Password = "ivan", Phone = "+7 912 367-57-97", RoleId = 2 }
               );

            builder.Entity<Product>()
                .HasOne(p => p.Manufacturer)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.ManufId);
            builder.Entity<Category>()
                .HasMany(c => c.SubCategories)
                .WithOne(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryId);
            builder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
            builder.Entity<User>()
               .HasOne(u => u.Cart)
               .WithOne(c => c.User)
               .HasForeignKey<Cart>(c => c.UserId);
        }
    }
}
