using OrderManagement.Web.Models;

namespace OrderManagement.Web.ViewModels;

public class OrderSummaryViewModel
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public int ItemCount { get; set; }
    public decimal Total { get; set; }
}
