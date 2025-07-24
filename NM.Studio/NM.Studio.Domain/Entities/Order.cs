using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }

    // Pricing
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }

    // Voucher/Promotion
    public string? VoucherCode { get; set; }
    public Guid? VoucherId { get; set; }

    // Dates
    public DateTimeOffset OrderDate { get; set; }
    public DateTimeOffset? ProcessedDate { get; set; }
    public DateTimeOffset? CompletedDate { get; set; }
    public DateTimeOffset? CancelledDate { get; set; }

    // Shipping Details
    public string ShippingAddress { get; set; } = string.Empty;
    public string ShippingCity { get; set; } = string.Empty;
    public string ShippingState { get; set; } = string.Empty;
    public string ShippingZipCode { get; set; } = string.Empty;
    public string ShippingCountry { get; set; } = string.Empty;
    public string? TrackingNumber { get; set; }
    public ShippingMethod ShippingMethod { get; set; }

    // Contact Info
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;

    // Notes
    public string? CustomerNotes { get; set; }
    public string? InternalNotes { get; set; }

    // Relationships
    public virtual User User { get; set; } = null!;
    public virtual Voucher? Voucher { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}