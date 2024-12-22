using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductXColors;

public class ProductXColorDeleteCommand : DeleteCommand
{
    public Guid? ProductId { get; set; }

    public Guid? ColorId { get; set; }
}