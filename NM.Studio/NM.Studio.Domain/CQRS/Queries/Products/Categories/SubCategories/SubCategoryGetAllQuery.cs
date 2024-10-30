using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.Products.Categories.SubCategories;

public class SubCategoryGetAllQuery : GetAllQuery
{
    public string? Name { get; set; }
}