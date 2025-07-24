using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.AlbumMedias;

public class AlbumMediaUpdateCommand : UpdateCommand
{
    public Guid? AlbumId { get; set; }

    public Guid? MediaFileId { get; set; }
}