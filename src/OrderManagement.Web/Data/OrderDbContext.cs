using Microsoft.EntityFrameworkCore;
using OrderManagement.Web.Models;

namespace OrderManagement.Web.Data;

/// <summary>
/// EF Core mapping for the AtDriveOrders database. The schema itself is
/// created by Database/AtDriveOrders.sql — the configuration below only
/// tells EF how the entities map onto those existing tables (column sizes,
/// the Status enum stored as a string, decimal precision, relationships).
/// </summary>
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(o => o.OrderId);
            entity.Property(o => o.CustomerName).HasMaxLength(100).IsRequired();
            entity.Property(o => o.Status)
                  .HasConversion<string>()
                  .HasMaxLength(20);

            entity.HasMany(o => o.Items)
                  .WithOne(i => i.Order)
                  .HasForeignKey(i => i.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems");
            entity.HasKey(i => i.OrderItemId);
            entity.Property(i => i.ProductName).HasMaxLength(100).IsRequired();
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2);
        });
    }
}
