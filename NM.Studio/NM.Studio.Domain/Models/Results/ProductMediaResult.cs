using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ProductMediaResult : BaseResult
{
    public Guid? MediaBaseId { get; set; }
    public Guid? ProductVariantId { get; set; }
    public ProductVariantResult? ProductVariant { get; set; }
    public MediaBaseResult? MediaBase { get; set; }
}