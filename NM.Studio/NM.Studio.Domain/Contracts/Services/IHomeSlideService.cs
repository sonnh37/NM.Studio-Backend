using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.HomeSlides;
using NM.Studio.Domain.Models.CQRS.Queries.HomeSlides;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IHomeSlideService : IBaseService
{
    Task<BusinessResult> GetAll(HomeSlideGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(HomeSlideGetByIdQuery request);
    Task<BusinessResult> Delete(HomeSlideDeleteCommand command);
}