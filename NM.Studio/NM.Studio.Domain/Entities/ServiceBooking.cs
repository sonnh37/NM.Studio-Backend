using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Entities;

public class ServiceBooking : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ServiceId { get; set; }
    public string BookingNumber { get; set; } = string.Empty;
    public ServiceBookingStatus Status { get; set; }

    // Timing
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
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;

    // Additional Details
    public string? SpecialRequirements { get; set; }
    public string? StaffNotes { get; set; }
    public string? CancellationReason { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Service Service { get; set; } = null!;
}