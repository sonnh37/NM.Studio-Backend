using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductXPhotos;

public class ProductXPhotoDeleteCommand : DeleteCommand
{
    public Guid? ProductId { get; set; }

    public Guid? PhotoId { get; set; }
}