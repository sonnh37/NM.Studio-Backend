using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.SubCategories;

public class SubCategoryCreateCommand : CreateCommand
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsFeatured { get; set; }

    public Guid? CategoryId { get; set; }
}