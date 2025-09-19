using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.ServiceBookings;
using NM.Studio.Domain.CQRS.Queries.ServiceBookings;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IServiceBookingService : IBaseService
{
    Task<BusinessResult> GetAll(ServiceBookingGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(ServiceBookingGetByIdQuery request);
    Task<BusinessResult> Delete(ServiceBookingDeleteCommand command);
    Task<BusinessResult> Cancel(ServiceBookingCancelCommand cancelCommand);
}