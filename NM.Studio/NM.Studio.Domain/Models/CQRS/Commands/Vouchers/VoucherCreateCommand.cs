using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.Vouchers;

public class VoucherCreateCommand : CreateCommand
{
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public VoucherType Type { get; set; }
    public VoucherStatus Status { get; set; }

    public decimal DiscountAmount { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal MinimumSpend { get; set; }
    public decimal MaximumDiscount { get; set; }

    public int MaxUsage { get; set; }
    public int MaxUsagePerUser { get; set; }
    public int UsageCount { get; set; }

    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }

    public bool IsFirstOrderOnly { get; set; }
    public string? ApplicableProductIds { get; set; }
    public string? ApplicableCategories { get; set; }
    public decimal? MaximumSpend { get; set; }

    public bool IsCombinableWithOther { get; set; }
    public bool IsPublic { get; set; }
    public string? UserGroupRestrictions { get; set; }
}