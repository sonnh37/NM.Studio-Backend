using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.Albums;

public class AlbumSetCoverUpdateCommand : UpdateCommand
{
    public Guid AlbumId { get; set; }
    public Guid ImageId { get; set; }
}