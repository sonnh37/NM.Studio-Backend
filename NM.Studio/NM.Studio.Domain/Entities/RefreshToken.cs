using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid? UserId { get; set; }

    public string? Token { get; set; }
    
    public string? UserAgent { get; set; }
    
    public string? IpAddress { get; set; }

    public DateTimeOffset? Expiry { get; set; }

    public virtual User? User { get; set; }
}