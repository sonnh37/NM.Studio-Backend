using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class ProductMedia : BaseEntity
{
    public Guid? MediaBaseId { get; set; }
    public Guid? ProductVariantId { get; set; }
    public virtual ProductVariant? ProductVariant { get; set; }
    public virtual MediaBase? MediaBase { get; set; }
}