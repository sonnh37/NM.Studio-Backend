using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class SubCategory : BaseEntity
{
    public string? Name { get; set; } // e.g., "Wedding Dresses", "Evening Gowns"
    public string? DisplayName { get; set; } // Formatted name for display
    public string? Slug { get; set; } // URL-friendly version of name
    public string? Description { get; set; } // Detailed description
    public string? ShortDescription { get; set; } // Brief description for listings
    public string? ImageUrl { get; set; } // Category image
    public bool IsActive { get; set; } // Whether category is active
    public int SortOrder { get; set; } // Display order in menus/lists
    public string? MetaTitle { get; set; } // SEO title
    public string? MetaDescription { get; set; } // SEO description
    public bool IsFeatured { get; set; } // Show in featured sections

    // Existing relationships
    public Guid? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}