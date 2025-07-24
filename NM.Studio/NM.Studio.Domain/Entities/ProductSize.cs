using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class ProductSize : BaseEntity
{
    public Guid? ProductId { get; set; }

    public Guid? SizeId { get; set; }

    public bool IsActive { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Size? Size { get; set; }
}