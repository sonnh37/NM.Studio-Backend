using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.CQRS.Commands.OrderStatusHistories;

public class OrderStatusHistoryDeleteCommand : DeleteCommand
{
    public Guid? OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public OrderStatus? PreviousStatus { get; set; }
    public string? Comment { get; set; }

    // Additional tracking
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Location { get; set; }

    // System fields
    public bool IsCustomerNotified { get; set; }
    public string? NotificationError { get; set; }
}