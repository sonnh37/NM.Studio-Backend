using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ProductResult : BaseResult
{
    public string? Sku { get; set; }
    
    public string? Slug { get; set; }

    public Guid? SubCategoryId { get; set; }

    public Guid? SizeId { get; set; }

    public Guid? ColorId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public ColorResult? Color { get; set; }

    public SubCategoryResult? SubCategory { get; set; }

    public SizeResult? Size { get; set; }

    public ProductStatus Status { get; set; }

    public List<ProductXPhotoResult> ProductXPhotos { get; set; } = new();
}

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