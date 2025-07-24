using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Entities;

public class OrderStatusHistory : BaseEntity
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public OrderStatus? PreviousStatus { get; set; }
    public string? Comment { get; set; }
    public string? ChangedBy { get; set; } // User who made the change
    public DateTimeOffset ChangedAt { get; set; }

    // Additional tracking
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Location { get; set; }

    // System fields
    public bool IsCustomerNotified { get; set; }
    public string? NotificationError { get; set; }

    public virtual Order Order { get; set; } = null!;
}