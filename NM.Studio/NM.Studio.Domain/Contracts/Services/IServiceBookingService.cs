using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.ServiceBookings;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IServiceBookingService : IBaseService
{
    Task<BusinessResult> Create<TResult>(ServiceBookingCreateCommand createCommand) where TResult : BaseResult;

    Task<BusinessResult> Cancel<TResult>(ServiceBookingCancelCommand cancelCommand) where TResult : BaseResult;
}