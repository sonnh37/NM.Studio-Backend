using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductImages;

public class ProductMediaDeleteCommand : DeleteCommand
{
    public Guid ImageId { get; set; }
    public Guid ProductVariantId { get; set; }
}