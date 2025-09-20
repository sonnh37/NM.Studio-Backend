using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.OrderItems;

public class OrderItemUpdateCommand : UpdateCommand
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
}