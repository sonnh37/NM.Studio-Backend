using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.SubCategories;

public class SubCategoryGetAllQuery : GetAllQuery
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsFeatured { get; set; }

    public Guid? CategoryId { get; set; }
}