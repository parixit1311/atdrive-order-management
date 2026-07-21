using System.ComponentModel.DataAnnotations;
using OrderManagement.Web.Models;

namespace OrderManagement.Web.ViewModels;

public class OrderFormViewModel
{
    public int OrderId { get; set; }

    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(100)]
    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [Display(Name = "Order Date")]
    public DateTime OrderDate { get; set; } = DateTime.Today;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [MinLength(1, ErrorMessage = "An order needs at least one line item.")]
    public List<OrderItemFormViewModel> Items { get; set; } = new();
}
