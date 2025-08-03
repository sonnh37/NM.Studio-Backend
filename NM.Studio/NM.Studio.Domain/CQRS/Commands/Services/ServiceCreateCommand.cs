using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Services;

public class ServiceCreateCommand : CreateCommand
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public string? Src { get; set; }
    public decimal? Price { get; set; }

    // Loại dịch vụ (chụp ảnh, trang điểm, cho thuê váy, ...)
    public string? Category { get; set; }

    // Dịch vụ này có hiển thị trang chủ không
    public bool IsFeatured { get; set; }

    // Trạng thái kích hoạt
    public bool IsActive { get; set; } = true;

    // Sắp xếp dịch vụ
    public int SortOrder { get; set; }

    // Hình minh họa/ảnh đại diện cho dịch vụ
    public string? ImageUrl { get; set; }

    // Mô tả ngắn (hiện danh sách)
    public string? ShortDescription { get; set; }

    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }

    // Điều kiện - lưu ý cho khách hàng khi đặt
    public string? TermsAndConditions { get; set; }

    // Số lượng lượt đặt tối đa trong ngày
    public int? MaxBookingsPerDay { get; set; }
}