using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid? OrderId { get; set; }
    public string? TransactionId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";

    public DateTimeOffset PaymentDate { get; set; }
    public DateTimeOffset? ProcessedDate { get; set; }

    public string? BillingName { get; set; }
    public string? BillingEmail { get; set; }
    public string? BillingPhone { get; set; }
    public string? BillingAddress { get; set; }

    public string? PaymentProviderResponse { get; set; }
    public string? FailureReason { get; set; }

    public virtual Order? Order { get; set; }
}

public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    BankTransfer,
    DigitalWallet,
    Cash,
    CryptoCurrency
}

public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Refunded,
    PartiallyRefunded,
    Cancelled
}