using OrderManagement.Web.Models;
using OrderManagement.Web.Repositories;
using OrderManagement.Web.ViewModels;

namespace OrderManagement.Web.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<OrderSummaryViewModel>> GetOrdersAsync(OrderStatus? status = null, string? customerName = null)
    {
        List<Order> orders;

        if (status.HasValue)
        {
            orders = await _repository.GetByStatusAsync(status.Value);

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                orders = orders
                    .Where(o => o.CustomerName.Contains(customerName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
        else
        {
            orders = await _repository.GetAllAsync(customerName);
        }

        return orders
            .Select(o => new OrderSummaryViewModel
            {
                OrderId = o.OrderId,
                CustomerName = o.CustomerName,
                OrderDate = o.OrderDate,
                Status = o.Status,
                ItemCount = o.Items.Count,
                Total = CalculateOrderTotal(o)
            })
            .ToList();
    }

    public Task<Order?> GetOrderAsync(int orderId)
    {
        return _repository.GetByIdAsync(orderId);
    }

    public async Task<Order> CreateOrderAsync(OrderFormViewModel form)
    {
        var order = new Order
        {
            CustomerName = form.CustomerName.Trim(),
            OrderDate = form.OrderDate,
            Status = form.Status,
            Items = form.Items
                .Select(i => new OrderItem
                {
                    ProductName = i.ProductName.Trim(),
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })
                .ToList()
        };

        return await _repository.AddAsync(order);
    }

    public async Task<bool> UpdateOrderAsync(OrderFormViewModel form)
    {
        var order = await _repository.GetByIdAsync(form.OrderId);
        if (order is null)
        {
            return false;
        }

        order.CustomerName = form.CustomerName.Trim();
        order.OrderDate = form.OrderDate;
        order.Status = form.Status;

        var submittedIds = form.Items
            .Where(i => i.OrderItemId != 0)
            .Select(i => i.OrderItemId)
            .ToHashSet();

        var removedItems = order.Items.Where(i => !submittedIds.Contains(i.OrderItemId)).ToList();
        foreach (var removed in removedItems)
        {
            order.Items.Remove(removed);
        }

        foreach (var formItem in form.Items)
        {
            var existing = formItem.OrderItemId != 0
                ? order.Items.FirstOrDefault(i => i.OrderItemId == formItem.OrderItemId)
                : null;

            if (existing is null)
            {
                order.Items.Add(new OrderItem
                {
                    ProductName = formItem.ProductName.Trim(),
                    Quantity = formItem.Quantity,
                    UnitPrice = formItem.UnitPrice
                });
            }
            else
            {
                existing.ProductName = formItem.ProductName.Trim();
                existing.Quantity = formItem.Quantity;
                existing.UnitPrice = formItem.UnitPrice;
            }
        }

        await _repository.UpdateAsync(order);
        return true;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await _repository.GetByIdAsync(orderId);
        if (order is null)
        {
            return false;
        }

        await _repository.DeleteAsync(order);
        return true;
    }

    public decimal CalculateOrderTotal(Order order)
    {
        return order.Items.Sum(i => i.Quantity * i.UnitPrice);
    }
}
