using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.AlbumImages;

public class AlbumImageDeleteCommand : DeleteCommand
{
    public Guid ImageId { get; set; }
    public Guid AlbumId { get; set; }
}