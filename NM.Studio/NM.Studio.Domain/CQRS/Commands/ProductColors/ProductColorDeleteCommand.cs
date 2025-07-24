using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductColors;

public class ProductColorDeleteCommand : DeleteCommand
{
    public Guid? ProductId { get; set; }

    public Guid? ColorId { get; set; }
}