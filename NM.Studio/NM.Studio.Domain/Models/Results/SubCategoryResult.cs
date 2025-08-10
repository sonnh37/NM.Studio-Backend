using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class SubCategoryResult : BaseResult
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
    public CategoryResult? Category { get; set; }
    public ICollection<ProductResult> Products { get; set; } = new List<ProductResult>();
}