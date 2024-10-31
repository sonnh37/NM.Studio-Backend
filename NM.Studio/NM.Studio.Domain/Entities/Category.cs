using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Category : BaseEntity
{
    public string? Name { get; set; }
    
    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

}