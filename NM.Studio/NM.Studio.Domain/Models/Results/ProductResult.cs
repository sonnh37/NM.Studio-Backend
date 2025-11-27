using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ProductResult : BaseResult
{
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? SubCategoryId { get; set; }
    public Guid? ThumbnailId { get; set; }
    public string? Description { get; set; }
    public string? Material { get; set; }
    public ProductStatus Status { get; set; }
    public CategoryResult? Category { get; set; }
    public SubCategoryResult? SubCategory { get; set; }
    public MediaBaseResult? Thumbnail { get; set; }
    public ICollection<ProductVariantResult> Variants { get; set; } = new List<ProductVariantResult>();
}

public class ProductPreviewResult : BaseResult
{
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? SubCategoryId { get; set; }
    public Guid? ThumbnailId { get; set; }
    public string? Description { get; set; }
    public string? Material { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal totalStockQuantity { get; set; }
    public decimal totalStockDefaultQuantity { get; set; }
    public ProductStatus Status { get; set; }
    public CategoryResult? Category { get; set; }
    public SubCategoryResult? SubCategory { get; set; }
    public MediaBaseResult? Thumbnail { get; set; }
    public ICollection<ProductVariantResult> Variants { get; set; } = new List<ProductVariantResult>();

    public decimal GetMinPrice()
    {
        return Variants.Count == 0 ? 0 : Variants.Min(n => n.Price ?? 0);
    }
    public decimal GetMaxPrice()
    {
        return Variants.Count == 0 ? 0 : Variants.Max(n => n.Price ?? 0);
    }}

public class ProductRepresentativeByCategoryResult
{
    public CategoryResult? Category { get; set; }

    public ProductRepresentativeResult? Product { get; set; }
}

public class ProductRepresentativeResult : BaseResult
{
    public string? Sku { get; set; }

    public string? Slug { get; set; }

    public string? Src { get; set; }
}