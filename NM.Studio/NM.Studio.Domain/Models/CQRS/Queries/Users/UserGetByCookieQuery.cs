using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Queries.Users;

public class UserGetByCookieQuery : IRequest<BusinessResult>
{
}