using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.CQRS.Commands.Users;

public class UserRefreshTokenCommand : IRequest<BusinessResult>
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}