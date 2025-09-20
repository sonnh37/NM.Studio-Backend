using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.Albums;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Queries.Albums;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IAlbumService : IBaseService
{
    Task<BusinessResult> GetAll(AlbumGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(AlbumGetByIdQuery request);
    Task<BusinessResult> Delete(AlbumDeleteCommand command);
}