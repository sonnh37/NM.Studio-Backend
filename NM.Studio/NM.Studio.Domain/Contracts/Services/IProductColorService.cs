using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.ProductColors;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IProductColorService : IBaseService
{
    Task<BusinessResult> DeleteById(ProductColorDeleteCommand command);

    Task<BusinessResult> Update<TResult>(ProductColorUpdateCommand updateCommand) where TResult : BaseResult;
}