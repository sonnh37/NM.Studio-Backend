using NM.Studio.Domain.Models.CQRS.Queries.Base;

namespace NM.Studio.Domain.Models.CQRS.Queries.Products;

public class ProductGetBySlugQuery : BaseQuery
{
    public string Slug { get; set; }
}