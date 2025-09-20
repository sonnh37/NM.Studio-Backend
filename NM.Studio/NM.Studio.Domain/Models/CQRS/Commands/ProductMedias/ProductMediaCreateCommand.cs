using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.ProductMedias;

public class ProductMediaCreateCommand : CreateCommand
{
    public Guid? MediaBaseId { get; set; }
    public Guid? ProductVariantId { get; set; }
}