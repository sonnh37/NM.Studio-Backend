using MediatR;
using NM.Studio.Domain.Models.Responses;

namespace NM.Studio.Domain.CQRS.Queries.Users;

public class UserGetByCookieQuery : IRequest<BusinessResult>
{
}