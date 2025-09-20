using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Commands.Users;

public class UserPasswordCommand : IRequest<BusinessResult>
{
    public string? Password { get; set; }
}