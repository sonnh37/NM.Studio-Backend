using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class MediaBaseResult : BaseResult
{
    public string? DisplayName { get; set; }
    public string? Title { get; set; }
    public string? MimeType { get; set; }
    public long Size { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? CreatedMediaBy { get; set; }
    public DateTimeOffset? TakenMediaDate { get; set; }
    public string? MediaUrl { get; set; }
    public MediaBaseType? MediaBaseType { get; set; }
}