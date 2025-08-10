using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.CQRS.Queries.OrderStatusHistories;

public class OrderStatusHistoryGetAllQuery : GetAllQuery
{
    public Guid? OrderId { get; set; }
    public OrderStatus? Status { get; set; }
    public OrderStatus? PreviousStatus { get; set; }
    public string? Comment { get; set; }

    // Additional tracking
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Location { get; set; }

    // System fields
    public bool? IsCustomerNotified { get; set; }
    public string? NotificationError { get; set; }
}