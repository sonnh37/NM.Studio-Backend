using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.ProductXColors;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IProductXColorService : IBaseService
{
    Task<BusinessResult> DeleteById(ProductXColorDeleteCommand command);

    Task<BusinessResult> Update<TResult>(ProductXColorUpdateCommand updateCommand) where TResult : BaseResult;
}