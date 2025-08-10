using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.CQRS.Queries.Payments;

public class PaymentGetByIdQuery : GetByIdQuery
{
    public Guid? OrderId { get; set; }
    public string? TransactionId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public PaymentStatus? Status { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }

    public DateTimeOffset? PaymentDate { get; set; }
    public DateTimeOffset? ProcessedDate { get; set; }

    public string? BillingName { get; set; }
    public string? BillingEmail { get; set; }
    public string? BillingPhone { get; set; }
    public string? BillingAddress { get; set; }

    public string? PaymentProviderResponse { get; set; }
    public string? FailureReason { get; set; }
}