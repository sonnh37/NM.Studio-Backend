using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductImages;

public class ProductImageDeleteCommand : DeleteCommand
{
    public Guid ImageId { get; set; }
    public Guid ProductVariantId { get; set; }
}