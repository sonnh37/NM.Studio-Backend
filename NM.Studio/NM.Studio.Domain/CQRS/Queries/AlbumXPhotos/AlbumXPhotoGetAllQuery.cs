using NM.Studio.Domain.CQRS.Queries.Albums;
using NM.Studio.Domain.CQRS.Queries.MediaFiles;

namespace NM.Studio.Domain.CQRS.Queries.AlbumXPhotos;

public class AlbumMediaGetAllQuery
{
    public Guid? AlbumId { get; set; }

    public Guid? MediaFileId { get; set; }

    public AlbumGetAllQuery? Album { get; set; }

    public MediaFileGetAllQuery? MediaFile { get; set; }
}