using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.ProductMedias;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IProductMediaService : IBaseService
{
    Task<BusinessResult> DeleteById(ProductMediaDeleteCommand command);
}