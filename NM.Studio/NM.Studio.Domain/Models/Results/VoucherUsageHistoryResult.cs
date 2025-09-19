using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class VoucherUsageHistoryResult : BaseResult
{
    public Guid? VoucherId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? OrderId { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTimeOffset UsedDate { get; set; }
    public VoucherResult? Voucher { get; set; }
    public UserResult? User { get; set; }
    public OrderResult? Order { get; set; }
}