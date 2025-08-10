using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class OrderItemResult : BaseResult
{
    public Guid? OrderId { get; set; }
    public Guid? ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? SelectedSize { get; set; }
    public string? SelectedColor { get; set; }
    public string? CustomizationNotes { get; set; }

    public OrderResult? Order { get; set; }
    public ProductResult? Product { get; set; }
}