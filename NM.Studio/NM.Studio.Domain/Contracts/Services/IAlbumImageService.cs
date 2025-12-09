using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.AlbumImages;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IAlbumImageService : IBaseService
{
    // Task<BusinessResult> GetAll(AlbumImageGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);

    // Task<BusinessResult> GetById(AlbumImageGetByIdQuery request);
    Task<BusinessResult> Delete(AlbumImageDeleteCommand command);
    Task<BusinessResult> CreateList(List<AlbumImageCreateCommand> commands);
    Task<BusinessResult> DeleteList(List<AlbumImageDeleteCommand> commands);
}