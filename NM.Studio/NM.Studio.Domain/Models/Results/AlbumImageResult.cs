using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class AlbumImageResult : BaseResult
{
    public string? ImageId { get; set; }
    public string? AlbumId { get; set; }
    public AlbumResult? Album { get; set; }
    public MediaBaseResult? Image { get; set; }
}