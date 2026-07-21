using OrderManagement.Web.Models;

namespace OrderManagement.Web.ViewModels;

public class OrderIndexViewModel
{
    public List<OrderSummaryViewModel> Orders { get; set; } = new();
    public OrderStatus? StatusFilter { get; set; }
    public string? CustomerNameFilter { get; set; }
}
