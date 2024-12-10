using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid? UserId { get; set; }
    
    public Guid? ServiceId { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public DateTime? BookingDate { get; set; }
    
    public BookingStatus? Status { get; set; }
    
    public virtual User? User { get; set; }
    
    public virtual Service? Service { get; set; }
    
    
}