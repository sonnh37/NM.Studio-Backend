using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ProductVariantResult : BaseResult
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
    public ProductResult? Product { get; set; }
    public ICollection<ProductMediaResult> ProductImages { get; set; } = new List<ProductMediaResult>();
}