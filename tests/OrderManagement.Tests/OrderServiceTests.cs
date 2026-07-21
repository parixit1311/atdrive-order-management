using Moq;
using OrderManagement.Web.Models;
using OrderManagement.Web.Repositories;
using OrderManagement.Web.Services;

namespace OrderManagement.Tests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repositoryMock = new();
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _service = new OrderService(_repositoryMock.Object);
    }

    [Fact]
    public void CalculateOrderTotal_SumsQuantityTimesUnitPrice()
    {
        var order = new Order
        {
            Items =
            {
                new OrderItem { ProductName = "Wireless Mouse", Quantity = 2, UnitPrice = 24.99m },
                new OrderItem { ProductName = "USB-C Hub", Quantity = 1, UnitPrice = 45.50m },
                new OrderItem { ProductName = "HDMI Cable", Quantity = 3, UnitPrice = 7.25m }
            }
        };

        var total = _service.CalculateOrderTotal(order);

        Assert.Equal(117.23m, total);
    }

    [Fact]
    public void CalculateOrderTotal_ReturnsZero_WhenOrderHasNoItems()
    {
        var order = new Order();

        var total = _service.CalculateOrderTotal(order);

        Assert.Equal(0m, total);
    }

    [Fact]
    public async Task GetOrderAsync_ReturnsNull_WhenOrderDoesNotExist()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Order?)null);

        var order = await _service.GetOrderAsync(999);

        Assert.Null(order);
    }

    [Fact]
    public async Task DeleteOrderAsync_ReturnsFalse_AndDoesNotDelete_WhenOrderDoesNotExist()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Order?)null);

        var deleted = await _service.DeleteOrderAsync(999);

        Assert.False(deleted);
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task GetOrdersAsync_MapsOrdersToSummaries_WithTotals()
    {
        _repositoryMock
            .Setup(r => r.GetAllAsync(null))
            .ReturnsAsync(new List<Order>
            {
                new()
                {
                    OrderId = 1,
                    CustomerName = "Alice Johnson",
                    Status = OrderStatus.Pending,
                    Items =
                    {
                        new OrderItem { Quantity = 2, UnitPrice = 10m },
                        new OrderItem { Quantity = 1, UnitPrice = 5.50m }
                    }
                }
            });

        var summaries = await _service.GetOrdersAsync();

        var summary = Assert.Single(summaries);
        Assert.Equal(1, summary.OrderId);
        Assert.Equal("Alice Johnson", summary.CustomerName);
        Assert.Equal(2, summary.ItemCount);
        Assert.Equal(25.50m, summary.Total);
    }
}
