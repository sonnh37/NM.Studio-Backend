using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductColors;

public class ProductColorUpdateCommand : UpdateCommand
{
    public Guid? ProductId { get; set; }

    public Guid? ColorId { get; set; }

    public bool IsActive { get; set; }
}