using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.ProductSizes;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IProductSizeService : IBaseService
{
    Task<BusinessResult> Delete(ProductSizeDeleteCommand command);
    Task<BusinessResult> Update<TResult>(ProductSizeUpdateCommand updateCommand) where TResult : BaseResult;
}