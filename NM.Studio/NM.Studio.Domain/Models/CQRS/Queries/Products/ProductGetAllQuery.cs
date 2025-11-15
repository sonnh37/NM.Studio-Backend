using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Queries.Base;

namespace NM.Studio.Domain.Models.CQRS.Queries.Products;

public class ProductGetAllQuery : GetAllQuery
{
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? SubCategoryId { get; set; }
    public string? Description { get; set; }
    public string? Material { get; set; }
    public ProductStatus? Status { get; set; }
}