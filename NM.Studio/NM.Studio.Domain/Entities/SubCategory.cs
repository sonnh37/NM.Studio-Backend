using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class SubCategory : BaseEntity
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsFeatured { get; set; }

    public Guid? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
}