using Microsoft.EntityFrameworkCore;
using SalesHub.Domain.Entities;

namespace SalesHub.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();
    public DbSet<User> Users => Set<User>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Price).HasColumnType("numeric(18,2)");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Total).HasColumnType("numeric(18,2)");
            entity.HasMany(x => x.Items)
                  .WithOne(i => i.Sale)
                  .HasForeignKey(i => i.SaleId);
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Subtotal).HasColumnType("numeric(18,2)");
            entity.HasOne(x => x.Product)
                  .WithMany(p => p.SaleItems)
                  .HasForeignKey(x => x.ProductId);
        });
    }
}
