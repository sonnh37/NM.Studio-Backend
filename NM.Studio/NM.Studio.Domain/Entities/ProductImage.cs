using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class ProductImage : BaseEntity
{
    public Guid? ImageId { get; set; }
    public Guid? ProductVariantId { get; set; }
    public virtual ProductVariant? ProductVariant { get; set; }
    public virtual Image? Image { get; set; }
}