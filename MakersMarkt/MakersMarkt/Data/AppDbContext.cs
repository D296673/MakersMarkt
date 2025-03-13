using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using MakersMarkt.Data;

namespace MakersMarkt.Data
{
    class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ModerationFlag> ModerationFlags { get; set; }
        public DbSet<Type> Types { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
            "server=localhost;user=root;password=;database=MakersMarkt",
            ServerVersion.Parse("8.0.30")
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Moderator"
                },
                new Role
                {
                    Id = 2,
                    Name = "Maker"
                },
                new Role
                {
                    Id = 3,
                    Name = "User"
                }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "mod",
                    Password = LoginPage.HashPassword("Mod"),
                    RoleId = 1,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    Id = 2,
                    Name = "maker",
                    Password = LoginPage.HashPassword("Maker"),
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    Id = 3,
                    Name = "user",
                    Password = LoginPage.HashPassword("User"),
                    RoleId = 3,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    Id = 4,
                    Name = "maker2",
                    Password = LoginPage.HashPassword("Maker2"),
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    Id = 5,
                    Name = "maker3",
                    Password = LoginPage.HashPassword("Maker3"),
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                }
                );
            modelBuilder.Entity<Type>().HasData(
                new Type
                {
                    Id = 1,
                    Name = "Sieraden"
                },
                new Type
                {
                    Id = 2,
                    Name = "Keramiek"
                },
                new Type
                {
                    Id = 3,
                    Name = "Textiel"
                },
                new Type
                {
                    Id = 4,
                    Name = "Kunst"
                }
            );

            var productFaker = new Faker<Product>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.TypeId, f => f.Random.Number(1, 4))
                .RuleFor(p => p.Material, f => f.Commerce.ProductMaterial())
                .RuleFor(p => p.ProductionTime, f => f.Date.Past())
                .RuleFor(p => p.Complexity, f => f.Commerce.ProductAdjective())
                .RuleFor(p => p.Durability, f => f.Random.Number(1, 10))
                .RuleFor(p => p.UniqueFeatures, f => f.Commerce.ProductAdjective())
                .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
                .RuleFor(p => p.CreatedAt, f => f.Date.Past())
                .RuleFor(p => p.Status, f => f.PickRandom("Available", "Unavailable"))
                .RuleFor(p => p.MakerId, f => f.Random.Number(1, 5))
                .RuleFor(p => p.Category, f => f.PickRandom(new[] { "Electronics", "Clothing", "Home & Kitchen", "Toys", "Books" }))
                .Generate(100);
            modelBuilder.Entity<Product>().HasData(productFaker);

            var Credits = new Faker<Credit>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
                .RuleFor(c => c.UserId, f => f.Random.Number(1, 5))
                .RuleFor(c => c.Balance, f => f.Random.Decimal(1, 1000))
                .RuleFor(c => c.UpdatedAt, f => f.Date.Past())
                .Generate(5);
            modelBuilder.Entity<Credit>().HasData(Credits);
            var Orders = new Faker<Order>()
                .RuleFor(o => o.Id, f => f.IndexFaker + 1)
                .RuleFor(o => o.ProductId, f => f.Random.Number(1, 100))
                .RuleFor(o => o.BuyerId, f => f.Random.Number(1, 5))
                .RuleFor(o => o.SellerId, f => f.Random.Number(1, 5))
                .RuleFor(o => o.Status, f => f.PickRandom("Pending", "Accepted", "Declined"))
                .RuleFor(o => o.CreatedAt, f => f.Date.Past())
                .Generate(10);
            modelBuilder.Entity<Order>().HasData(Orders);
            var Reviews = new Faker<Review>()
                .RuleFor(r => r.Id, f => f.IndexFaker + 1)
                .RuleFor(r => r.ProductId, f => f.Random.Number(1, 100))
                .RuleFor(r => r.UserId, f => f.Random.Number(1, 5))
                .RuleFor(r => r.Rating, f => f.Random.Number(1, 5))
                .RuleFor(r => r.Comment, f => f.Lorem.Sentence())
                .RuleFor(r => r.CreatedAt, f => f.Date.Past())
                .Generate(10);
            modelBuilder.Entity<Review>().HasData(Reviews);
            var notificationFaker = new Faker<Notification>()
                .RuleFor(n => n.Id, f => f.IndexFaker + 1)
                .RuleFor(n => n.UserId, f => f.Random.Number(1, 5))
                .RuleFor(n => n.OrderId, f => f.Random.Bool() ? (int?)f.Random.Number(1, 10) : null) // 50% chance to fill OrderId
                .RuleFor(n => n.ReviewId, f => f.Random.Bool() ? null : (int?)f.Random.Number(1, 10)) // 50% chance to fill ReviewId
                .RuleFor(n => n.Message, f => f.Lorem.Sentence())
                .RuleFor(n => n.Type, f => f.PickRandom("Order", "Review"))
                .RuleFor(n => n.CreatedAt, f => f.Date.Past())
                .Generate(10);
            modelBuilder.Entity<Notification>().HasData(notificationFaker);

            var moderationFaker = new Faker<ModerationFlag>()
                .RuleFor(m => m.Id, f => f.IndexFaker + 1)
                .RuleFor(m => m.ProductId, f => f.Random.Number(1, 100))
                .RuleFor(m => m.UserId, f => f.Random.Number(1, 5))
                .RuleFor(m => m.ReviewId, f => f.Random.Number(1, 10)) // Initially fill ReviewId
                .RuleFor(m => m.ModeratorId, f => f.Random.Number(1, 5))
                .RuleFor(m => m.Reason, f => f.Lorem.Sentence())
                .RuleFor(m => m.Category, f => f.PickRandom("Spam", "Inappropriate", "Offensive"))
                .RuleFor(m => m.CreatedAt, f => f.Date.Past())
                .Generate(10);
            modelBuilder.Entity<ModerationFlag>().HasData(moderationFaker);
        }
    }
}