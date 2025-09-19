using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.Carts;
using NM.Studio.Domain.CQRS.Queries.Carts;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface ICartService : IBaseService
{
    Task<BusinessResult> GetAll(CartGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(CartGetByIdQuery request);
    Task<BusinessResult> Delete(CartDeleteCommand command);
}