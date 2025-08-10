using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.VoucherUsageHistories;

public class VoucherUsageHistoryDeleteCommand : DeleteCommand
{
    public Guid? VoucherId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? OrderId { get; set; }
    public decimal? DiscountAmount { get; set; }
    public DateTimeOffset? UsedDate { get; set; }
}