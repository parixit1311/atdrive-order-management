using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Web.ViewModels;

public class OrderItemFormViewModel
{
    public int OrderItemId { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100)]
    [Display(Name = "Product")]
    public string ProductName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
    public int Quantity { get; set; } = 1;

    [Range(0.01, 999999999, ErrorMessage = "Unit price must be a positive amount.")]
    [Display(Name = "Unit Price")]
    public decimal UnitPrice { get; set; }
}
