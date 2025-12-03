using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.ProductMedias;

public class ProductMediaDeleteCommand : DeleteCommand
{
    public Guid MediaBaseId { get; set; }
    public Guid ProductVariantId { get; set; }
}