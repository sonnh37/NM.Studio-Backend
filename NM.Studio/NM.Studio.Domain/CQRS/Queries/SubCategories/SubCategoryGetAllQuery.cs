using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.SubCategories;

public class SubCategoryGetAllQuery : GetAllQuery
{
    public string? Name { get; set; }
    
    public Guid? CategoryId { get; set; }
    
    public bool? IsNullCategoryId { get; set; }
    
}