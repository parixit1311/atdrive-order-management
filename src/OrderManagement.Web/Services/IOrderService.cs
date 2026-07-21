using OrderManagement.Web.Models;
using OrderManagement.Web.ViewModels;

namespace OrderManagement.Web.Services;

public interface IOrderService
{
    Task<List<OrderSummaryViewModel>> GetOrdersAsync(OrderStatus? status = null, string? customerName = null);
    Task<Order?> GetOrderAsync(int orderId);
    Task<Order> CreateOrderAsync(OrderFormViewModel form);
    Task<bool> UpdateOrderAsync(OrderFormViewModel form);
    Task<bool> DeleteOrderAsync(int orderId);
    decimal CalculateOrderTotal(Order order);
}
