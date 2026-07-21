using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Web.Models;

public class Order
{
    public int OrderId { get; set; }

    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
