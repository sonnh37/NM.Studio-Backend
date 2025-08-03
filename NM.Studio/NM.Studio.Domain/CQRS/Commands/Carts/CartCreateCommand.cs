using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Carts;

public class CartCreateCommand: CreateCommand
{
    public Guid? UserId { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
    public string? VoucherCode { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
}