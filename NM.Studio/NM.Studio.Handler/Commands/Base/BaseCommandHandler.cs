using MediatR;
using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Handler.Commands.Base;

public abstract class BaseCommandHandler
{
    protected readonly IBaseService _baseService;

    protected BaseCommandHandler(IBaseService baseService)
    {
        _baseService = baseService;
    }
}