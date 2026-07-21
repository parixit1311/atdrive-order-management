using Microsoft.EntityFrameworkCore;
using OrderManagement.Web.Models;

namespace OrderManagement.Web.Data;

public static class DbInitializer
{
    public static void Initialize(OrderDbContext context)
    {
        context.Database.Migrate();

        if (context.Orders.Any())
        {
            return;
        }

        var orders = new List<Order>
        {
            new()
            {
                CustomerName = "Alice Johnson",
                OrderDate = DateTime.Today.AddDays(-10),
                Status = OrderStatus.Completed,
                Items =
                {
                    new OrderItem { ProductName = "Wireless Mouse", Quantity = 2, UnitPrice = 24.99m },
                    new OrderItem { ProductName = "USB-C Hub", Quantity = 1, UnitPrice = 45.50m }
                }
            },
            new()
            {
                CustomerName = "Bob Martinez",
                OrderDate = DateTime.Today.AddDays(-3),
                Status = OrderStatus.Processing,
                Items =
                {
                    new OrderItem { ProductName = "Mechanical Keyboard", Quantity = 1, UnitPrice = 89.00m }
                }
            },
            new()
            {
                CustomerName = "Charlotte Lee",
                OrderDate = DateTime.Today.AddDays(-1),
                Status = OrderStatus.Pending,
                Items =
                {
                    new OrderItem { ProductName = "27\" Monitor", Quantity = 2, UnitPrice = 179.99m },
                    new OrderItem { ProductName = "HDMI Cable", Quantity = 3, UnitPrice = 7.25m }
                }
            }
        };

        context.Orders.AddRange(orders);
        context.SaveChanges();
    }
}
