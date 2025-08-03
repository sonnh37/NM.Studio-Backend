using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class CartItem : BaseEntity
{
    public Guid? CartId { get; set; }
    public Guid? ProductId { get; set; }
    public int Quantity { get; set; }
    public string? SelectedSize { get; set; }
    public string? SelectedColor { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public virtual Cart? Cart { get; set; } 
    public virtual Product? Product { get; set; }
}