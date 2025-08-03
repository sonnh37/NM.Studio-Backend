using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.CartItems;

public class CartItemUpdateCommand : UpdateCommand
{
    public Guid? CartId { get; set; }
    public Guid? ProductId { get; set; }
    public int Quantity { get; set; }
    public string? SelectedSize { get; set; }
    public string? SelectedColor { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

}