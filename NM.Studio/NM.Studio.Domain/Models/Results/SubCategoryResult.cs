namespace NM.Studio.Domain.Models.Results;

public class SubCategoryResult
{
    public string? Name { get; set; }

    public Guid? CategoryId { get; set; }

    public CategoryResult? Category { get; set; }
    
    public List<ProductResult> Products { get; set; } = new();

}