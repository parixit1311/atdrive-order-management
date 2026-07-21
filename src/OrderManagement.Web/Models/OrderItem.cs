using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Web.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100)]
    public string ProductName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
    public int Quantity { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be a positive amount.")]
    public decimal UnitPrice { get; set; }

    public Order? Order { get; set; }
}
