using System.Runtime.Serialization;
using System.Security.AccessControl;
using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class MediaBase : BaseEntity
{
    public string? DisplayName { get; set; }
    public string? Title { get; set; }
    public string? Format { get; set; }
    public long Size { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? CreatedMediaBy { get; set; }
    public DateTimeOffset? TakenMediaDate { get; set; }
    public string? MediaUrl { get; set; }
    public ResourceType? ResourceType { get; set; }

    public string GetTypeFile()
    {
        return ResourceType + "/" + Format;
    }
}

public enum ResourceType
{
    /// <summary>Images in various formats (jpg, png, etc.)</summary>
    [EnumMember(Value = "image")] Image,
    /// <summary>Any files (text, binary)</summary>
    [EnumMember(Value = "raw")] Raw,
    /// <summary>Video files in various formats (mp4, etc.)</summary>
    [EnumMember(Value = "video")] Video,
    /// <summary>Auto upload format</summary>
    [EnumMember(Value = "auto")] Auto,
}