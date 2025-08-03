using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class MediaFileResult : BaseResult
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsFeatured { get; set; }

    public string? Type { get; set; }

    public string? Src { get; set; }

    public string? Href { get; set; }

    public Guid? AlbumId { get; set; }

    public Guid? ProductId { get; set; }

    public List<AlbumMediaResult> AlbumMedias { get; set; } = new();

    public List<ProductMediaResult> ProductMedias { get; set; } = new();
}