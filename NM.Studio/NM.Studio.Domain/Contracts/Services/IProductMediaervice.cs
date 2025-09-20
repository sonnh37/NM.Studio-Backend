using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.ProductMedias;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IProductMediaervice : IBaseService
{
    // Task<BusinessResult> GetAll(ProductImageGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);

    // Task<BusinessResult> GetById(ProductImageGetByIdQuery request);
    Task<BusinessResult> Delete(ProductMediaDeleteCommand command);
}