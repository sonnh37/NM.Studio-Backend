using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Category : BaseEntity
{
    public string? Name { get; set; }
    
    public string? DisplayName { get; set; }
    
    public string? Slug { get; set; }
    
    public string? Description { get; set; }
    
    public string? ShortDescription { get; set; }
    
    public string? IconUrl { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public string? ThumbnailUrl { get; set; }
    
    public bool IsActive { get; set; }
    
    public bool IsFeatured { get; set; }
    
    public int SortOrder { get; set; }

    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}