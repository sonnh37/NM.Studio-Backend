using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class AlbumImageResult : BaseResult
{
    public int SortOrder { get; set; }
    public bool IsCover { get; set; }
    public bool IsThumbnail { get; set; }
    public string? ImageId { get; set; }
    public string? AlbumId { get; set; }
    public AlbumResult? Album { get; set; }
    public ImageResult? Image { get; set; }
}