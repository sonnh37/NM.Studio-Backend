using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Commands.Users;

public class UserUpdateCacheCommand : IRequest<BusinessResult>
{
    public string? Cache { get; set; }
}