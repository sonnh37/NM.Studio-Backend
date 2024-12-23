﻿using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class BookingResult : BaseResult
{
    public Guid? UserId { get; set; }
    
    public Guid? ServiceId { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public DateTime? BookingDate { get; set; }
    
    public BookingStatus? Status { get; set; }
}