using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.CQRS.Commands.Users;

public class UserCreateByGoogleTokenCommand : IRequest<BusinessResult>
{
    public string? Token { get; set; }
    public string? Password { get; set; }
}