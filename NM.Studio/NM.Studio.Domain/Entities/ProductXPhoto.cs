using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class ProductXPhoto : BaseEntity
{
    public Guid? ProductId { get; set; }

    public Guid? PhotoId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Photo? Photo { get; set; }
}