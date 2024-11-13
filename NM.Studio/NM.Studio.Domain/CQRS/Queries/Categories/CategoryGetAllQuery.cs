using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.Categories;

public class CategoryGetAllQuery : GetAllQuery
{
    public string? Name { get; set; }
}