using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.CQRS.Queries.Products;

public class ProductGetAllQuery : GetAllQuery
{
    public string? Sku { get; set; }

    public Guid? SubCategoryId { get; set; }

    public Guid? SizeId { get; set; }

    public Guid? ColorId { get; set; }
    
    public string? Slug { get; set; }
    
    public bool IsNotNullSlug { get; set; }
    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public ProductStatus Status { get; set; }

    public Guid? CategoryId { get; set; }
    
    public string? CategoryName { get; set; }
    
    public string? SubCategoryName { get; set; }
    
    public List<string> Colors { get; set; } = new List<string>();
    public List<string> Sizes { get; set; } = new List<string>();
}