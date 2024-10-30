using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class CategoryResult : BaseResult
{
    public string? Name { get; set; }

    public List<ProductResult> Products { get; set; } = new();
    public List<SubCategoryResult> SubCategories { get; set; } = new();
}