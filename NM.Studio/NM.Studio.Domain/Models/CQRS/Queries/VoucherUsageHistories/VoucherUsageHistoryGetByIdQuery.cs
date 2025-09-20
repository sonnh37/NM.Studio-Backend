using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.VoucherUsageHistories;

public class VoucherUsageHistoryGetByIdQuery : GetByIdQuery
{
    public Guid? VoucherId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? OrderId { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTimeOffset UsedDate { get; set; }
}