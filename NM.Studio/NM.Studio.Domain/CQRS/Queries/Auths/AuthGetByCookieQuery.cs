using MediatR;
using NM.Studio.Domain.Models.Responses;

namespace NM.Studio.Domain.CQRS.Queries.Auths;

public class AuthGetByCookieQuery : IRequest<BusinessResult>
{
}