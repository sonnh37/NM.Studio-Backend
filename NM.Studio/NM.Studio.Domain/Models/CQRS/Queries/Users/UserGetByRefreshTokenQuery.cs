using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.CQRS.Queries.Users;

public class UserGetByRefreshTokenQuery : IRequest<BusinessResult>
{
    public string? RefreshToken { get; set; }
}