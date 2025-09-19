using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class UserOtp : BaseEntity
{
    public Guid? UserId { get; set; }
    public string Code { get; set; } = null!;
    public OtpType Type { get; set; }
    public OtpStatus Status { get; set; } 
    public DateTimeOffset ExpiredAt { get; set; }
    public DateTimeOffset? VerifiedAt { get; set; }

    public virtual User? User { get; set; }
}

public enum OtpStatus
{
    Pending = 0,
    Verified = 1,
    Expired = 2,
    Failed = 3
}

public enum OtpType
{
    Email = 0,
    SMS = 1,
    TwoFactor = 2
}