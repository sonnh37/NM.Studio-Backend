using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface ISubCategoryService : IBaseService
{
    Task<BusinessResult> GetAll(GetQueryableQuery query);
}