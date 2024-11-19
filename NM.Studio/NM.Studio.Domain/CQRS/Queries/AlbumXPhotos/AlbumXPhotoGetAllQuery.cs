using NM.Studio.Domain.CQRS.Queries.Albums;
using NM.Studio.Domain.CQRS.Queries.Photos;

namespace NM.Studio.Domain.CQRS.Queries.AlbumXPhotos;

public class AlbumXPhotoGetAllQuery
{
    public Guid? AlbumId { get; set; }

    public Guid? PhotoId { get; set; }

    public AlbumGetAllQuery? Album { get; set; }

    public PhotoGetAllQuery? Photo { get; set; }
}