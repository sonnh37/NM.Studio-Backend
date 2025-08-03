using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Commands.Users;

public class UserLogoutCommand : IRequest<BusinessResult>
{
    public string? RefreshToken { get; set; }
}