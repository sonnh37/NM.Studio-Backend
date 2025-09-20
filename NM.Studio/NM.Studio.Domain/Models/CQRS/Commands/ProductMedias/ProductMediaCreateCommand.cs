using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductImages;

public class ProductMediaCreateCommand : CreateCommand
{
    public string? ImageId { get; set; }
    public string? ProductVariantId { get; set; }
}