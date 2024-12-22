using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Size : BaseEntity
{
    public string? Name { get; set; }

    public virtual ICollection<ProductXSize> ProductXSizes { get; set; } = new List<ProductXSize>();
}