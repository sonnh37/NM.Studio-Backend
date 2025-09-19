using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ImageResult : BaseResult
{
    public string? Description { get; set; }
    public Guid? MediaBaseId { get; set; }
    public Guid? MediaUrlId { get; set; }
    public MediaBaseResult? MediaBase { get; set; }
    public MediaUrlResult? MediaUrl { get; set; }
}