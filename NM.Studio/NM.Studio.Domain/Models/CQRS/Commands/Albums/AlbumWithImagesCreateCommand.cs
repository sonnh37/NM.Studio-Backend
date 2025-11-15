using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.Albums;

public class AlbumWithImagesCreateCommand : CreateCommand
{
    public Guid? AlbumId { get; set; }
    public List<Guid> ImageIds { get; set; } 
}