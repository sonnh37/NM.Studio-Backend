namespace NM.Studio.Domain.Enums;

public enum ShippingMethod
{
    Standard,
    Express,
    NextDay,
    PickupInStore
}

public enum OrderStatus
{
    Pending, // Order placed but not confirmed
    Confirmed, // Order confirmed, awaiting processing
    Processing, // Order is being processed
    ReadyForShipment, // Order processed and ready for shipping
    Shipped, // Order has been shipped
    Delivered, // Order has been delivered
    Completed, // Order fully completed
    Cancelled, // Order was cancelled
    Refunded, // Order was refunded
    OnHold // Order is on hold
}