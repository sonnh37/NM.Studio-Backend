using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.OrderItems;

public class OrderItemGetAllQuery : GetAllQuery
{
    public Guid? OrderId { get; set; }
    public Guid? ProductId { get; set; }
    public int? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Subtotal { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? SelectedSize { get; set; }
    public string? SelectedColor { get; set; }
    public string? CustomizationNotes { get; set; }
}