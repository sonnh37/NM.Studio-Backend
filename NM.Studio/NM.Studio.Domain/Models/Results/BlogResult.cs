using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class BlogResult : BaseResult
{
    public string? Title { get; set; } // Tiêu đề blog
    public string? Slug { get; set; } // Đường dẫn thân thiện URL
    public string? Content { get; set; } // Nội dung blog
    public bool IsFeatured { get; set; } // Đánh dấu là bài viết nổi bật (VD: "About" cho trang home)
    public string? Thumbnail { get; set; }
}