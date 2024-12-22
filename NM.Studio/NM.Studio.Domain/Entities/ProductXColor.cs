using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class ProductXColor : BaseEntity
{
    public Guid? ProductId { get; set; }
    
    public Guid? ColorId { get; set; }
    
    public bool IsActive { get; set; }

    public virtual Product? Product { get; set; }
    
    public virtual Color? Color { get; set; }
}
