using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.ProductMedias;

public class ProductMediaUpdateCommand : UpdateCommand
{
    public Guid? MediaBaseId { get; set; }
    public Guid? ProductVariantId { get; set; }
}