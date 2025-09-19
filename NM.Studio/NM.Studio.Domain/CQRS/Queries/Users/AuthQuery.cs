using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Queries.Users;

public class AuthQuery : IRequest<BusinessResult>
{
    public string Account { get; set; }
    public string Password { get; set; }
}