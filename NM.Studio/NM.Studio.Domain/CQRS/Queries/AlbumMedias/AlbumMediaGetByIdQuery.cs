using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.AlbumMedias;

public class AlbumMediaGetByIdQuery : GetByIdQuery
{
    public Guid? AlbumId { get; set; }

    public Guid? MediaFileId { get; set; }
}