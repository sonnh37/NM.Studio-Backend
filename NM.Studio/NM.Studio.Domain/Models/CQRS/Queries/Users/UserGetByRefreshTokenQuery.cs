using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Queries.Users;

public class UserGetByRefreshTokenQuery : IRequest<BusinessResult>
{
    public string? RefreshToken { get; set; }
}