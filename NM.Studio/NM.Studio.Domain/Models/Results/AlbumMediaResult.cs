using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class AlbumMediaResult : BaseResult
{
    public Guid? AlbumId { get; set; }

    public Guid? MediaFileId { get; set; }

    public AlbumResult? Album { get; set; }

    public MediaFileResult? MediaFile { get; set; }
}