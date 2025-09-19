using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.Videos;
using NM.Studio.Domain.CQRS.Queries.Videos;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IVideoService : IBaseService
{
    Task<BusinessResult> GetAll(VideoGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(VideoGetByIdQuery request);
    Task<BusinessResult> Delete(VideoDeleteCommand command);
}