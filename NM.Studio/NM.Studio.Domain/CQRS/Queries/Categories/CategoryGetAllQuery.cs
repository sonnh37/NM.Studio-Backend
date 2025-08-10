using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.Categories;

public class CategoryGetAllQuery : GetAllQuery
{
    public string? Name { get; set; }
    
    public string? DisplayName { get; set; }
    
    public string? Slug { get; set; }
    
    public string? Description { get; set; }
    
    public string? ShortDescription { get; set; }
    
    public string? IconUrl { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public string? ThumbnailUrl { get; set; }
    
    public bool? IsActive { get; set; }
    
    public bool? IsFeatured { get; set; }
    
    public int? SortOrder { get; set; }
}