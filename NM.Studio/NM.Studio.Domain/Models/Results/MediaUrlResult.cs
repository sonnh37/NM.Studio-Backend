using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class MediaUrlResult : BaseResult
{
    public string? UrlInternal { get; set; }
    public string? UrlExternal { get; set; }

    public ImageResult? Image { get; set; }
    public VideoResult? Video { get; set; }
}