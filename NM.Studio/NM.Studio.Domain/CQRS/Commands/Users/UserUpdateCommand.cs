using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.CQRS.Commands.Users;

public class UserUpdateCommand : UpdateCommand
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FullName { get; set; }

    public string? Avatar { get; set; }

    public string? Email { get; set; }

    public DateTimeOffset? Dob { get; set; }

    public string? Address { get; set; }

    public Gender? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public Role? Role { get; set; }

    public UserStatus? Status { get; set; }

    public string? Cache { get; set; }

    public string? Otp { get; set; }

    public DateTimeOffset? OtpExpiration { get; set; }

    // Account security and verification
    public bool IsEmailVerified { get; set; }
    public bool IsPhoneVerified { get; set; }
    public bool TwoFactorEnabled { get; set; }

    // Audit fields
    public DateTimeOffset? LastLoginDate { get; set; }
    public string? LastLoginIp { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }

    // Additional user info
    public string? Nationality { get; set; }
    public string? PreferredLanguage { get; set; }
    public string? TimeZone { get; set; }

    // Profile settings
    public DateTimeOffset? PasswordChangedDate { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTimeOffset? PasswordResetExpiration { get; set; }
}