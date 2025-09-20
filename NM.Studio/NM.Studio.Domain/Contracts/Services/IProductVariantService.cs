using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.ProductVariants;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IProductVariantService : IBaseService
{
    // Task<BusinessResult> GetAll(ProductVariantGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);

    // Task<BusinessResult> GetById(ProductVariantGetByIdQuery request);
    Task<BusinessResult> Delete(ProductVariantDeleteCommand command);
}