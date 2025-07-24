using MediatR;
using NM.Studio.Domain.Models.Responses;

namespace NM.Studio.Domain.CQRS.Queries.Users;

public class UserGetByRefreshTokenQuery : IRequest<BusinessResult>
{
    public string? RefreshToken { get; set; }
}