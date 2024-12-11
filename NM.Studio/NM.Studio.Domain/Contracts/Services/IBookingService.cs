using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Bookings;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IBookingService : IBaseService
{
    Task<BusinessResult> Create<TResult>(BookingCreateCommand createCommand) where TResult : BaseResult;

    Task<BusinessResult> Cancel<TResult>(BookingCancelCommand cancelCommand) where TResult : BaseResult;
}