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

public enum VoucherType
{
    FixedAmount, // Fixed amount off (e.g., $10 off)
    Percentage, // Percentage off (e.g., 15% off)
    FreeShipping, // Free shipping
    BuyOneGetOne, // BOGO deals
    FirstOrder, // First order discount
    ReferralReward, // Referral program voucher
    Seasonal, // Seasonal promotion
    BirthdaySpecial, // Birthday special discount
    LoyaltyReward, // Loyalty program reward
    WelcomeBack // Re-engagement campaign
}

public enum VoucherStatus
{
    Active, // Voucher is active and can be used
    Inactive, // Voucher is temporarily inactive
    Expired, // Voucher has expired
    FullyRedeemed, // Maximum usage limit reached
    Cancelled, // Voucher was cancelled
    Scheduled, // Scheduled for future activation
    Draft // In draft/preview mode
}