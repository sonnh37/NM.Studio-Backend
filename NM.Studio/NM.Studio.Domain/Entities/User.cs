using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public enum UserStatus
{
    Active,
    Inactive,
    Suspended
}

public enum Role
{
    Admin,
    Staff,
    Customer
}

public enum Gender
{
    Male,
    Female,
    Other
}

public class User : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? DisplayName { get; set; }
    public Guid? AvatarId { get; set; }
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

    // Navigation
    public virtual MediaBase? Avatar { get; set; }
    public virtual UserSetting? UserSetting { get; set; }
    public virtual ICollection<UserOtp> UserOtps { get; set; } = new List<UserOtp>();
    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
