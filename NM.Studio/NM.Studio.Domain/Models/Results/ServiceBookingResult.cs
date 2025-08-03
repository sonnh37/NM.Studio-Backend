using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ServiceBookingResult : BaseResult
{
    public Guid? UserId { get; set; }

    public Guid? ServiceId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTimeOffset? BookingDate { get; set; }

    public ServiceBookingResult? Status { get; set; }
}