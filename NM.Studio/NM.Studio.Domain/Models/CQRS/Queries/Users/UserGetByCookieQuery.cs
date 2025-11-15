using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.CQRS.Queries.Users;

public class UserGetByCookieQuery : IRequest<BusinessResult>
{
}