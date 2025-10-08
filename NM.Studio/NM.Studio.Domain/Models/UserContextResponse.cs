using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models;

public class UserContextResponse : BaseResult
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? Dob { get; set; }
    public string? Address { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public Role? Role { get; set; }
    public UserStatus? Status { get; set; }

    // Verification
    public bool IsEmailVerified { get; set; }
    public bool IsPhoneVerified { get; set; }
    public bool TwoFactorEnabled { get; set; }

    // Profile
    public string? Nationality { get; set; }
    public string? PreferredLanguage { get; set; }
    public string? TimeZone { get; set; }
}