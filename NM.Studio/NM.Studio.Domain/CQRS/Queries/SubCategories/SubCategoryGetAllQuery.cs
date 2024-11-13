using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.SubCategories;

public class SubCategoryGetAllQuery : GetAllQuery
{
    public string? Name { get; set; }
}