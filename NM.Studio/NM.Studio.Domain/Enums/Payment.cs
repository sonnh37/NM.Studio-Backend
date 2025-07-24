namespace NM.Studio.Domain.Enums;

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