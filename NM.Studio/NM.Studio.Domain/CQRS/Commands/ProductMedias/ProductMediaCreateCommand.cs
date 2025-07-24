using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductMedias;

public class ProductMediaCreateCommand : CreateCommand
{
    public Guid? ProductId { get; set; }

    public Guid? MediaFileId { get; set; }
}