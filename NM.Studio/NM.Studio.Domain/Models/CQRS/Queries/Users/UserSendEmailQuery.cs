using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.CQRS.Queries.Users;

public class UserSendEmailQuery : IRequest<BusinessResult>
{
    public string? Email { get; set; }
}