using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.CQRS.Commands.ServiceBookings;

public class ServiceBookingUpdateCommand : UpdateCommand
{
    public Guid? UserId { get; set; }
    public Guid? ServiceId { get; set; }
    public string? BookingNumber { get; set; }
    public ServiceBookingStatus Status { get; set; }

    public DateTimeOffset AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int DurationMinutes { get; set; }

    // Pricing
    public decimal ServicePrice { get; set; }
    public decimal DepositAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsDepositPaid { get; set; }

    // Customer Info
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }

    // Additional Details
    public string? SpecialRequirements { get; set; }
    public string? StaffNotes { get; set; }
    public string? CancellationReason { get; set; }
}