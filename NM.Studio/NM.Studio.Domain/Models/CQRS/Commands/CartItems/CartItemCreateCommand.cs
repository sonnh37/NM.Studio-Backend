using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.CartItems;

public class CartItemCreateCommand : CreateCommand
{
    public Guid? CartId { get; set; }
    public Guid? ProductId { get; set; }
    public int Quantity { get; set; }
    public string? SelectedSize { get; set; }
    public string? SelectedColor { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}