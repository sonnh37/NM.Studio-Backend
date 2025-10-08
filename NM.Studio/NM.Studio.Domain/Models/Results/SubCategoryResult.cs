using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class SubCategoryResult : BaseResult
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    
    public bool IsFeatured { get; set; }

    public Guid? CategoryId { get; set; }
}