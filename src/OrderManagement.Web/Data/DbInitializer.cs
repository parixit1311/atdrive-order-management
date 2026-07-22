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
                CustomerName = "Lionel Messi",
                OrderDate = DateTime.Today.AddDays(-28),
                Status = OrderStatus.Completed,
                Items =
                {
                    new OrderItem { ProductName = "Home Jersey", Quantity = 2, UnitPrice = 89.99m },
                    new OrderItem { ProductName = "Captain's Armband", Quantity = 1, UnitPrice = 14.50m }
                }
            },
            new()
            {
                CustomerName = "Cristiano Ronaldo",
                OrderDate = DateTime.Today.AddDays(-24),
                Status = OrderStatus.Completed,
                Items =
                {
                    new OrderItem { ProductName = "Football Boots", Quantity = 1, UnitPrice = 219.00m },
                    new OrderItem { ProductName = "Training Bibs", Quantity = 5, UnitPrice = 9.99m }
                }
            },
            new()
            {
                CustomerName = "Neymar Jr",
                OrderDate = DateTime.Today.AddDays(-21),
                Status = OrderStatus.Cancelled,
                Items =
                {
                    new OrderItem { ProductName = "Away Jersey", Quantity = 3, UnitPrice = 84.99m }
                }
            },
            new()
            {
                CustomerName = "Kylian Mbappe",
                OrderDate = DateTime.Today.AddDays(-16),
                Status = OrderStatus.Shipped,
                Items =
                {
                    new OrderItem { ProductName = "Match Ball", Quantity = 4, UnitPrice = 39.99m },
                    new OrderItem { ProductName = "Shin Guards", Quantity = 2, UnitPrice = 19.95m }
                }
            },
            new()
            {
                CustomerName = "Erling Haaland",
                OrderDate = DateTime.Today.AddDays(-13),
                Status = OrderStatus.Processing,
                Items =
                {
                    new OrderItem { ProductName = "Goalkeeper Gloves", Quantity = 1, UnitPrice = 54.00m },
                    new OrderItem { ProductName = "Water Bottle", Quantity = 6, UnitPrice = 7.25m }
                }
            },
            new()
            {
                CustomerName = "Jude Bellingham",
                OrderDate = DateTime.Today.AddDays(-10),
                Status = OrderStatus.Pending,
                Items =
                {
                    new OrderItem { ProductName = "Training Cones (Set of 20)", Quantity = 2, UnitPrice = 24.99m }
                }
            },
            new()
            {
                CustomerName = "Vinicius Junior",
                OrderDate = DateTime.Today.AddDays(-8),
                Status = OrderStatus.Shipped,
                Items =
                {
                    new OrderItem { ProductName = "Home Jersey", Quantity = 1, UnitPrice = 89.99m },
                    new OrderItem { ProductName = "Team Scarf", Quantity = 2, UnitPrice = 16.50m }
                }
            },
            new()
            {
                CustomerName = "Lamine Yamal",
                OrderDate = DateTime.Today.AddDays(-5),
                Status = OrderStatus.Processing,
                Items =
                {
                    new OrderItem { ProductName = "Football Boots", Quantity = 1, UnitPrice = 179.00m },
                    new OrderItem { ProductName = "Grip Socks", Quantity = 4, UnitPrice = 12.99m }
                }
            },
            new()
            {
                CustomerName = "Mohamed Salah",
                OrderDate = DateTime.Today.AddDays(-3),
                Status = OrderStatus.Pending,
                Items =
                {
                    new OrderItem { ProductName = "Match Ball", Quantity = 2, UnitPrice = 39.99m }
                }
            },
            new()
            {
                CustomerName = "Kevin De Bruyne",
                OrderDate = DateTime.Today.AddDays(-1),
                Status = OrderStatus.Pending,
                Items =
                {
                    new OrderItem { ProductName = "Away Jersey", Quantity = 1, UnitPrice = 84.99m },
                    new OrderItem { ProductName = "Shin Guards", Quantity = 1, UnitPrice = 19.95m },
                    new OrderItem { ProductName = "Team Scarf", Quantity = 1, UnitPrice = 16.50m }
                }
            }
        };

        context.Orders.AddRange(orders);
        context.SaveChanges();
    }
}
