using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Web.Models;
using OrderManagement.Web.Services;
using OrderManagement.Web.ViewModels;

namespace OrderManagement.Web.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(OrderStatus? status, string? customerName)
    {
        var model = new OrderIndexViewModel
        {
            Orders = await _orderService.GetOrdersAsync(status, customerName),
            StatusFilter = status,
            CustomerNameFilter = customerName
        };

        return View(model);
    }

    public async Task<IActionResult> OrderTable(OrderStatus? status, string? customerName)
    {
        var orders = await _orderService.GetOrdersAsync(status, customerName);
        return PartialView("_OrderTable", orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return OrderNotFound(id);
        }

        ViewBag.Total = _orderService.CalculateOrderTotal(order);
        return View(order);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderFormViewModel form)
    {
        if (form.Items.Count == 0)
        {
            ModelState.AddModelError(nameof(form.Items), "An order needs at least one line item.");
        }

        if (!ModelState.IsValid)
        {
            return Json(new { success = false, errors = CollectModelErrors() });
        }

        var order = await _orderService.CreateOrderAsync(form);
        _logger.LogInformation("Order {OrderId} created for {CustomerName}.", order.OrderId, order.CustomerName);

        return Json(new { success = true, orderId = order.OrderId });
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return OrderNotFound(id);
        }

        var form = new OrderFormViewModel
        {
            OrderId = order.OrderId,
            CustomerName = order.CustomerName,
            OrderDate = order.OrderDate,
            Status = order.Status,
            Items = order.Items
                .Select(i => new OrderItemFormViewModel
                {
                    OrderItemId = i.OrderItemId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })
                .ToList()
        };

        return View(form);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(OrderFormViewModel form)
    {
        if (form.Items.Count == 0)
        {
            ModelState.AddModelError(nameof(form.Items), "An order needs at least one line item.");
        }

        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var updated = await _orderService.UpdateOrderAsync(form);
        if (!updated)
        {
            return OrderNotFound(form.OrderId);
        }

        TempData["Message"] = $"Order #{form.OrderId} was updated.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _orderService.DeleteOrderAsync(id);
        if (!deleted)
        {
            return OrderNotFound(id);
        }

        _logger.LogInformation("Order {OrderId} deleted by {User}.", id, User.Identity?.Name);
        TempData["Message"] = $"Order #{id} was deleted.";
        return RedirectToAction(nameof(Index));
    }

    private IActionResult OrderNotFound(int id)
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        ViewBag.OrderId = id;
        return View("NotFound");
    }

    private Dictionary<string, string[]> CollectModelErrors()
    {
        return ModelState
            .Where(kvp => kvp.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());
    }
}
