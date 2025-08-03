using NM.Studio.Domain.CQRS.Queries.Albums;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.CQRS.Queries.MediaFiles;

namespace NM.Studio.Domain.CQRS.Queries.AlbumMedias;

public class AlbumMediaGetAllQuery : GetAllQuery
{
    public Guid? AlbumId { get; set; }

    public Guid? MediaFileId { get; set; }

    public AlbumGetAllQuery? Album { get; set; }

    public MediaFileGetAllQuery? MediaFile { get; set; }
}