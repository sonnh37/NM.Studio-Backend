using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services.Bases;

// public interface IBaseService
// {
//     
// }
public interface IBaseService
{
    Task<BusinessResult> GetAll<TResult>() where TResult : BaseResult;

    Task<BusinessResult> GetListByQueryAsync<TResult>(GetQueryableQuery query) where TResult : BaseResult;

    Task<BusinessResult> GetById<TResult>(Guid id) where TResult : BaseResult;

    Task<BusinessResult> DeleteById(Guid id, bool isPermanent = false);

    Task<BusinessResult> CreateOrUpdate<TResult>(CreateOrUpdateCommand createOrUpdateCommand)
        where TResult : BaseResult;

    Task<BusinessResult> Restore<TResult>(UpdateCommand updateCommand) where TResult : BaseResult;
}