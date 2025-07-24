using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class ProductMedia : BaseEntity
{
    public Guid? ProductId { get; set; }

    public Guid? MediaFileId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual MediaFile? MediaFile { get; set; }
}