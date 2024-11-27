using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class AlbumResult : BaseResult
{
    public string? Title { get; set; }
    
    public string? Slug { get; set; }

    public string? Description { get; set; }

    public string? Background { get; set; }

    public List<AlbumXPhotoResult> AlbumXPhotos { get; set; } = new();
}