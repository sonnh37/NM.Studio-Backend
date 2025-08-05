using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.AlbumMedias;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IAlbumMediaService : IBaseService
{
    Task<BusinessResult> Delete(AlbumMediaDeleteCommand command);
}