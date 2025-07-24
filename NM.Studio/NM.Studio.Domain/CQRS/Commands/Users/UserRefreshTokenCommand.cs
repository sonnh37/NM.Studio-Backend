using MediatR;
using NM.Studio.Domain.Models.Responses;

namespace NM.Studio.Domain.CQRS.Commands.Users;

public class UserRefreshTokenCommand : IRequest<BusinessResult>
{
    public string? RefreshToken { get; set; }
}