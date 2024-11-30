using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Blog : BaseEntity
{
    public string? Title { get; set; }         // Tiêu đề blog
    public string? Slug { get; set; }          // Đường dẫn thân thiện URL
    public string? Content { get; set; }       // Nội dung blog
    public bool IsFeatured { get; set; }       // Đánh dấu là bài viết nổi bật (VD: "About" cho trang home)
    public string? Thumbnail { get; set; }     // Hình ảnh đại diện
}