using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductSizes;

public class ProductSizeUpdateCommand : UpdateCommand
{
    public Guid? ProductId { get; set; }

    public Guid? SizeId { get; set; }

    public bool? IsActive { get; set; }
}