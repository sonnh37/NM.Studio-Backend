using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.Orders;
using NM.Studio.Domain.Models.CQRS.Queries.Orders;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IOrderService : IBaseService
{
    Task<BusinessResult> GetAll(OrderGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(OrderGetByIdQuery request);
    Task<BusinessResult> Delete(OrderDeleteCommand command);
}