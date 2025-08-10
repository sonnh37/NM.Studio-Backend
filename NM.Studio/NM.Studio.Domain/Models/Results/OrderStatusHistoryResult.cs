using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class OrderStatusHistoryResult : BaseResult
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

    public OrderResult? Order { get; set; }
}