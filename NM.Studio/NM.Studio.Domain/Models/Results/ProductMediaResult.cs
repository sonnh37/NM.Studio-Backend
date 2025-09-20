using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ProductMediaResult : BaseResult
{
    public string? ImageId { get; set; }
    public string? ProductVariantId { get; set; }
    public virtual ProductVariant? ProductVariant { get; set; }
    public virtual Image? Image { get; set; }
}