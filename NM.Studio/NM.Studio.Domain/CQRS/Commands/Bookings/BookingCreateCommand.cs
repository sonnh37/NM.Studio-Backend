using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.CQRS.Commands.Bookings;

public class BookingCreateCommand : CreateCommand
{
    public Guid? UserId { get; set; }
    
    public Guid? ServiceId { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public DateTime? BookingDate { get; set; }
    
    public BookingStatus? Status { get; set; }
    
}