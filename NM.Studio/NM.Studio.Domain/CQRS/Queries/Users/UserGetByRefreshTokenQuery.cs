using MediatR;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.CQRS.Queries.Users;

public class UserGetByRefreshTokenQuery : IRequest<BusinessResult>
{
    public string? RefreshToken { get; set; }
}