using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductXSizes;

public class ProductXSizeDeleteCommand : DeleteCommand
{
    public Guid? ProductId { get; set; }

    public Guid? SizeId { get; set; }
}