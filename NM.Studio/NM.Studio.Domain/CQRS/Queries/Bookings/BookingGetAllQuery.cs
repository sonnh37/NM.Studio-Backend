using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.CQRS.Queries.Bookings;

public class BookingGetAllQuery : GetAllQuery
{
    public Guid? UserId { get; set; }
    
    public Guid? ServiceId { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public DateTime? BookingDate { get; set; }
    
    public BookingStatus? Status { get; set; }
}