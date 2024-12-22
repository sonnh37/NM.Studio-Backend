using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.CQRS.Commands.ProductXSizes;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IProductXSizeService : IBaseService
{
    Task<BusinessResult> DeleteById(ProductXSizeDeleteCommand command);
    Task<BusinessResult> Update<TResult>(ProductXSizeUpdateCommand updateCommand) where TResult : BaseResult;
}