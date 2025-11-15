using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.CartItems;
using NM.Studio.Domain.Models.CQRS.Queries.CartItems;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface ICartItemService : IBaseService
{
    Task<BusinessResult> GetAll(CartItemGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(CartItemGetByIdQuery request);
    Task<BusinessResult> Delete(CartItemDeleteCommand command);
}