using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<SaleItem> SaleItems => Set<SaleItem>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Cart> Carts => Set<Cart>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Sale
            modelBuilder.Entity<Sale>(e =>
            {
                e.HasKey(s => s.Id);
                e.Property(s => s.SaleNumber).IsRequired();
                e.Property(s => s.Customer).IsRequired();
                e.Property(s => s.Branch).IsRequired();
                e.Property(s => s.SaleDate).IsRequired();

                e.HasMany(s => s.Items)
                 .WithOne()
                 .HasForeignKey("SaleId")
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // SaleItem
            modelBuilder.Entity<SaleItem>(e =>
            {
                e.HasKey(i => i.Id);
                e.Property(i => i.ProductId).IsRequired();
                e.Property(i => i.Quantity).IsRequired();
                e.Property(i => i.UnitPrice).IsRequired();
                e.Property(i => i.Discount).HasDefaultValue(0);
            });

            // Product
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(p => p.Id);
                e.OwnsOne(p => p.Rating);
            });

            // User
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.OwnsOne(u => u.Name);
                e.OwnsOne(u => u.Address, addr =>
                {
                    addr.OwnsOne(a => a.Geolocation);
                });
            });

            // Cart
            modelBuilder.Entity<Cart>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Date).IsRequired();
                e.HasMany(c => c.Products).WithOne().HasForeignKey("CartId").OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(e =>
            {
                e.HasKey(ci => new { ci.ProductId, ci.Quantity }); // chave composta simplificada
            });
        }
    }
}
