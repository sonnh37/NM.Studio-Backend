using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductSizes;

public class ProductSizeDeleteCommand : DeleteCommand
{
    public Guid? ProductId { get; set; }

    public Guid? SizeId { get; set; }
}