using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.ProductXPhotos;

public class ProductXPhotoCreateCommand : CreateCommand
{
    public Guid? ProductId { get; set; }

    public Guid? PhotoId { get; set; }
}