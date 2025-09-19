using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class CategoryResult : BaseResult
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsFeatured { get; set; }

    public ICollection<SubCategoryResult> SubCategories { get; set; } = new List<SubCategoryResult>();
}