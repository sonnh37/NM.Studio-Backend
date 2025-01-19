using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.RefreshTokens;

public class RefreshTokenDeleteCommand : DeleteCommand
{
    public Guid? UserId { get; set; }

    public string? Token { get; set; }
    
    public string? UserAgent { get; set; }
    
    public string? IpAddress { get; set; }
}