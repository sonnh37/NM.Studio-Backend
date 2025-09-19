using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Queries.Products;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IProductService : IBaseService
{
    Task<BusinessResult> GetAll(ProductGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(ProductGetByIdQuery request);
    Task<BusinessResult> Delete(ProductDeleteCommand command);

    Task<BusinessResult> GetRepresentativeByCategory(ProductRepresentativeByCategoryQuery query);
}