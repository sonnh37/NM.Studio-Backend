using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services.Bases;

// public interface IBaseService
// {
//     
// }
public interface IBaseService
{
    Task<BusinessResult> GetAll<TResult>() where TResult : BaseResult;

    Task<BusinessResult> GetAll<TResult>(GetQueryableQuery query) where TResult : BaseResult;

    Task<BusinessResult> GetById<TResult>(GetByIdQuery id) where TResult : BaseResult;

    Task<BusinessResult> Delete(DeleteCommand deleteCommand);

    Task<BusinessResult> CreateOrUpdate<TResult>(CreateOrUpdateCommand createOrUpdateCommand)
        where TResult : BaseResult;

    Task<BusinessResult> Restore<TResult>(UpdateCommand updateCommand) where TResult : BaseResult;
}