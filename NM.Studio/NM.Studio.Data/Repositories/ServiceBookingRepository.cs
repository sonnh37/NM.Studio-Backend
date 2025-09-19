using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Data.Repositories;

public class ServiceBookingRepository : BaseRepository<ServiceBooking>, IServiceBookingRepository
{
    public ServiceBookingRepository(StudioContext dbContext) : base(dbContext)
    {
    }
}