using Microsoft.EntityFrameworkCore;
using OrderManagement.Web.Data;
using OrderManagement.Web.Models;

namespace OrderManagement.Web.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync(string? customerName = null)
    {
        var query = _context.Orders
            .Include(o => o.Items)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(customerName))
        {
            query = query.Where(o => o.CustomerName.Contains(customerName));
        }

        return await query
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<List<Order>> GetByStatusAsync(OrderStatus status)
    {
        // Stored procedures cannot be composed with Include(), so the
        // line items are loaded in a second query and stitched on.
        var orders = await _context.Orders
            .FromSqlInterpolated($"EXEC dbo.GetOrdersByStatus @Status = {status.ToString()}")
            .AsNoTracking()
            .ToListAsync();

        var orderIds = orders.Select(o => o.OrderId).ToList();
        var items = await _context.OrderItems
            .AsNoTracking()
            .Where(i => orderIds.Contains(i.OrderId))
            .ToListAsync();

        var itemsByOrder = items.ToLookup(i => i.OrderId);
        foreach (var order in orders)
        {
            order.Items = itemsByOrder[order.OrderId].ToList();
        }

        return orders;
    }

    public async Task<Order> AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task UpdateAsync(Order order)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Order order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }
}
