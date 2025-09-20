using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.UserTokens;

public class UserTokenCreateCommand : CreateCommand
{
    public Guid? UserId { get; set; }

    public string? Token { get; set; }

    public string? KeyId { get; set; }

    public string? PublicKey { get; set; }

    public string? UserAgent { get; set; }

    public string? IpAddress { get; set; }

    public DateTimeOffset? Expiry { get; set; }
}