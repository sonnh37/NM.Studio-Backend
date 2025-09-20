using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.VoucherUsageHistories;

public class VoucherUsageHistoryUpdateCommand : UpdateCommand
{
    public Guid? VoucherId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? OrderId { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTimeOffset UsedDate { get; set; }
}