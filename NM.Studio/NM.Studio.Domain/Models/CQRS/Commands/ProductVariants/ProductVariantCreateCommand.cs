using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.ProductVariants;

public class ProductVariantCreateCommand : CreateCommand
{
    public Guid? ProductId { get; set; }
    public string? Sku { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal? Price { get; set; }
    public decimal? RentalPrice { get; set; }
    public decimal? Deposit { get; set; }
    public int StockQuantity { get; set; }
    public ProductStatus Status { get; set; }
    public virtual Product? Product { get; set; }
}