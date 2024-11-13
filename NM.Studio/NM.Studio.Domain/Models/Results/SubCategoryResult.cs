using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class SubCategoryResult : BaseResult
{
    public string? Name { get; set; }

    public Guid? CategoryId { get; set; }

    public CategoryResult? Category { get; set; }
    
    public List<ProductResult> Products { get; set; } = new();

}