using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class SubCategory : BaseEntity
{
    public string? Name { get; set; }
    
    public Guid? CategoryId { get; set; }
    
    public virtual Category? Category { get; set; }
    
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}