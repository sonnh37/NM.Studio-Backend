using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.UserTokens;

public class UserTokenCreateCommand : CreateCommand
{
    public Guid? UserId { get; set; }

    public string? RefreshToken { get; set; }

    public string? UserAgent { get; set; }

    public string? IpAddress { get; set; }

    public DateTimeOffset? ExpiryTime { get; set; }
}