using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class VoucherUsageHistory : BaseEntity
{
    public Guid? VoucherId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? OrderId { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTimeOffset UsedDate { get; set; }
    public virtual Voucher? Voucher { get; set; }
    public virtual User? User { get; set; }
    public virtual Order? Order { get; set; }
}