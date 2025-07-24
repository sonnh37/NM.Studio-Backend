using NM.Studio.Domain.Contracts.Services.Bases;

namespace NM.Studio.Handler.Commands.Base;

public abstract class BaseCommandHandler
{
    protected readonly IBaseService _baseService;

    protected BaseCommandHandler(IBaseService baseService)
    {
        _baseService = baseService;
    }
}