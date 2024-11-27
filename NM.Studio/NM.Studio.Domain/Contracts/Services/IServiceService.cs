using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Services;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IServiceService : IBaseService
{
    Task<BusinessResult> Create<TResult>(ServiceCreateCommand createCommand) where TResult : BaseResult;
    Task<BusinessResult> Update<TResult>(ServiceUpdateCommand createCommand) where TResult : BaseResult;
}