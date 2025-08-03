using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.CQRS.Commands.Orders;

public class OrderUpdateCommand : UpdateCommand
{
    public Guid? UserId { get; set; }
    public string? OrderNumber { get; set; }
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
    public string? ShippingAddress { get; set; }
    public string? ShippingCity { get; set; }
    public string? ShippingState { get; set; }
    public string? ShippingZipCode { get; set; }
    public string? ShippingCountry { get; set; } 
    public string? TrackingNumber { get; set; }
    public ShippingMethod ShippingMethod { get; set; }

    // Contact Info
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }

    // Notes
    public string? CustomerNotes { get; set; }
    public string? InternalNotes { get; set; }
}