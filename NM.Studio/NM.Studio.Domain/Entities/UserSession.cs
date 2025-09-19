using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class UserSession : BaseEntity
{
    public Guid? UserId { get; set; }

    public DateTimeOffset LoginDate { get; set; }
    public string? LoginIp { get; set; }
    public string? DeviceInfo { get; set; }
    public string? SessionToken { get; set; }

    public DateTimeOffset? LastActivity { get; set; }
    public DateTimeOffset? Expiration { get; set; }

    public bool IsActive { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public virtual User? User { get; set; }
}
