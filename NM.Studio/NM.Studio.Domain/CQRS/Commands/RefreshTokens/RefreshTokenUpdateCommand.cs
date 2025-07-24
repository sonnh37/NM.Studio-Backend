using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.RefreshTokens;

public class RefreshTokenUpdateCommand : UpdateCommand
{
    public Guid? UserId { get; set; }

    public string? Token { get; set; }

    public string? PublicKey { get; set; }

    public string? UserAgent { get; set; }

    public string? IpAddress { get; set; }

    public DateTimeOffset? Expiry { get; set; }
}