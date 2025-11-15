using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class UserToken : BaseEntity
{
    public Guid? UserId { get; set; }

    public string? RefreshToken { get; set; }

    public string? UserAgent { get; set; }

    public string? IpAddress { get; set; }

    public DateTimeOffset? ExpiryTime { get; set; }

    public virtual User? User { get; set; }
}