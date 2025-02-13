﻿using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Entities;

public class User : BaseEntity
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Avatar  { get; set; }

    public string? Email { get; set; }

    public DateTime? Dob { get; set; }

    public string? Address { get; set; }

    public Gender? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public Role? Role { get; set; }

    public UserStatus? Status { get; set; }
    
    public string? Preferences { get; set; } 
    
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}