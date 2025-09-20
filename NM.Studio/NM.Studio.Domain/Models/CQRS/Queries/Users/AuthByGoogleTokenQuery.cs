using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.CQRS.Queries.Users;

public class AuthByGoogleTokenQuery : IRequest<BusinessResult>
{
    public string? Token { get; set; }
}