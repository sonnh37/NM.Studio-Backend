using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Commands.Users;

public class UserRefreshTokenCommand : IRequest<BusinessResult>
{
    public string? RefreshToken { get; set; }
}