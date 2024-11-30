using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Blogs;

public class BlogUpdateCommand : UpdateCommand
{
    public string? Title { get; set; }         // Tiêu đề blog
    public string? Slug { get; set; }          // Đường dẫn thân thiện URL
    public string? Content { get; set; }       // Nội dung blog
    public bool IsFeatured { get; set; }       // Đánh dấu là bài viết nổi bật (VD: "About" cho trang home)
    public string? Thumbnail { get; set; }
}