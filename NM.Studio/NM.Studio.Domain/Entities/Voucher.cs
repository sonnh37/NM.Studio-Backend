using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Entities;

public class Voucher : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public VoucherType Type { get; set; }
    public VoucherStatus Status { get; set; }

    // Discount Details
    public decimal DiscountAmount { get; set; } // Fixed amount discount
    public decimal DiscountPercentage { get; set; } // Percentage discount
    public decimal MinimumSpend { get; set; } // Minimum order amount
    public decimal MaximumDiscount { get; set; } // Maximum discount amount

    // Usage Limits
    public int MaxUsage { get; set; } // Maximum total uses
    public int MaxUsagePerUser { get; set; } // Maximum uses per user
    public int UsageCount { get; set; } // Current usage count

    // Validity Period
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    // Restrictions
    public bool IsFirstOrderOnly { get; set; } // Only for first orders
    public string? ApplicableProductIds { get; set; } // Comma-separated product IDs
    public string? ApplicableCategories { get; set; } // Comma-separated categories
    public decimal? MaximumSpend { get; set; } // Maximum order amount

    // Additional Settings
    public bool IsCombinableWithOther { get; set; } // Can be used with other vouchers
    public bool IsPublic { get; set; } // Visible to all users
    public string? UserGroupRestrictions { get; set; } // Specific user groups

    // Tracking
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<VoucherUsageHistory> VoucherUsageHistories { get; set; } = new List<VoucherUsageHistory>();
}