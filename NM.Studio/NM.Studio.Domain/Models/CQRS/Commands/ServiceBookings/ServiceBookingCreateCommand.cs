using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.ServiceBookings;

public class ServiceBookingCreateCommand : CreateCommand
{
    public Guid? UserId { get; set; }
    public Guid ServiceId { get; set; }
    public DateTimeOffset AppointmentDate { get; set; }
    // public TimeSpan StartTime { get; set; }
    // public TimeSpan EndTime { get; set; }

    // Customer Info
    public string? CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }

    // Additional Details
    public string? SpecialRequirements { get; set; }
    public string? StaffNotes { get; set; }
    public string? CancellationReason { get; set; }
}