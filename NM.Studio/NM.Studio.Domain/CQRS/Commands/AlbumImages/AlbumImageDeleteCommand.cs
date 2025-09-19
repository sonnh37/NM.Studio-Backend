using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.AlbumImages;

public class AlbumImageDeleteCommand : DeleteCommand
{
    public Guid ImageId { get; set; }
    public Guid AlbumId { get; set; }
}