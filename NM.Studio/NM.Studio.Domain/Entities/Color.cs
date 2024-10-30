using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Color : BaseEntity
{
    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}