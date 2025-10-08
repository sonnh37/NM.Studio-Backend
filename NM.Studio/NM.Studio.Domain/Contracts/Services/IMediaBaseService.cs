using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.MediaBases;
using NM.Studio.Domain.Models.CQRS.Queries.MediaBases;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IMediaBaseService : IBaseService
{
    Task<MediaBaseResult?> CreateMediaBaseFromSrc(string? src);
    Task<BusinessResult> GetAll(MediaBaseGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(MediaBaseGetByIdQuery request);
    Task<BusinessResult> Delete(MediaBaseDeleteCommand command);
}