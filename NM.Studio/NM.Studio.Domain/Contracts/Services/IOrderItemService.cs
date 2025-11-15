using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.OrderItems;
using NM.Studio.Domain.Models.CQRS.Queries.OrderItems;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IOrderItemService : IBaseService
{
    Task<BusinessResult> GetAll(OrderItemGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(OrderItemGetByIdQuery request);
    Task<BusinessResult> Delete(OrderItemDeleteCommand command);
}