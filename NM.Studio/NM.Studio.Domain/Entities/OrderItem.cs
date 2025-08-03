using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class OrderItem : BaseEntity
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

    public virtual Order? Order { get; set; } 
    public virtual Product? Product { get; set; } 
}