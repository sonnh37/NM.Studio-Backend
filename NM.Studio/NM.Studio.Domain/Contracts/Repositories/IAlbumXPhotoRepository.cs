using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.CQRS.Commands.AlbumXPhotos;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IAlbumXPhotoRepository : IBaseRepository<AlbumXPhoto>
{
    Task<AlbumXPhoto?> GetById(AlbumXPhotoDeleteCommand command);
    void Delete(AlbumXPhoto entity);
}