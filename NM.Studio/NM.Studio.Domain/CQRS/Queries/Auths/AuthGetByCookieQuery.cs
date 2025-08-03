using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Queries.Auths;

public class AuthGetByCookieQuery : IRequest<BusinessResult>
{
}