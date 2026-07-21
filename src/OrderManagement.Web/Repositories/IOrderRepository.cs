using OrderManagement.Web.Models;

namespace OrderManagement.Web.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync(string? customerName = null);
    Task<Order?> GetByIdAsync(int orderId);
    Task<List<Order>> GetByStatusAsync(OrderStatus status);
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(Order order);
}
