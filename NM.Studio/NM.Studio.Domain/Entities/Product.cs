using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Entities;

public class Product : BaseEntity
{
    public string? Sku { get; set; }
    public string? Slug { get; set; }
    public string? Name { get; set; }

    // Categorization
    public Guid? SubCategoryId { get; set; }
    public virtual SubCategory? SubCategory { get; set; }

    // Pricing and Availability
    public decimal? Price { get; set; }
    public decimal? RentalPrice { get; set; } // For rentable items
    public decimal? Deposit { get; set; } // For rentable items
    public bool IsRentable { get; set; }
    public bool IsSaleable { get; set; }

    // Details
    public string? Description { get; set; }
    public string? Material { get; set; }
    public string? Brand { get; set; }
    public string? Style { get; set; }
    public string? Care { get; set; } // Care instructions

    // Status
    public ProductStatus Status { get; set; }

    // Collections
    public virtual ICollection<ProductMedia> ProductMedias { get; set; } = new List<ProductMedia>();
    public virtual ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}

public enum ProductStatus
{
    // Trạng thái không xác định hoặc chưa thiết lập
    Unspecified,

    // Trang phục có sẵn để thuê hoặc bán
    Available,

    // Trang phục đã được thuê và không còn sẵn
    Rented,

    // Trang phục đang được bảo dưỡng hoặc sửa chữa
    InMaintenance,

    // Trang phục đã bán hoặc không còn được sử dụng
    Discontinued
}