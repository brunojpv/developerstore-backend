﻿using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<Sale> Sales => Set<Sale>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().OwnsOne(p => p.Rating);

            modelBuilder.Entity<User>().OwnsOne(u => u.Address, a =>
            {
                a.OwnsOne(g => g.Geolocation);
            });

            modelBuilder.Entity<User>().OwnsOne(u => u.Name);

            modelBuilder.Entity<Cart>().OwnsMany(c => c.Products, b =>
            {
                b.WithOwner().HasForeignKey("CartId");
                b.HasKey("ProductId", "CartId");
            });

            modelBuilder.Entity<Sale>().OwnsMany(s => s.Items, b =>
            {
                b.WithOwner().HasForeignKey("SaleId");
                b.HasKey("ProductId", "SaleId");
            });
        }
    }
}
