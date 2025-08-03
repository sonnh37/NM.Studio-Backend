using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Queries.Users;

public class DecodedTokenQuery : IRequest<BusinessResult>
{
    public string? Token { get; set; }
}