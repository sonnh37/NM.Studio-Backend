using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ProductResult : BaseResult
{
    public string? Sku { get; set; }

    public string? Slug { get; set; }

    public Guid? SubCategoryId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public SubCategoryResult? SubCategory { get; set; }

    public ProductStatus Status { get; set; }

    public ICollection<ProductMediaResult> ProductMedias { get; set; } = new List<ProductMediaResult>();

    public ICollection<ProductColorResult> ProductColors { get; set; } = new List<ProductColorResult>();

    public ICollection<ProductSizeResult> ProductSizes { get; set; } = new List<ProductSizeResult>();
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