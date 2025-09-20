using NM.Studio.Domain.Models.CQRS.Queries.Base;

namespace NM.Studio.Domain.Models.CQRS.Queries.CartItems;

public class CartItemGetAllQuery : GetAllQuery
{
    public Guid? CartId { get; set; }
    public Guid? ProductId { get; set; }
    public int? Quantity { get; set; }
    public string? SelectedSize { get; set; }
    public string? SelectedColor { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? TotalPrice { get; set; }
}