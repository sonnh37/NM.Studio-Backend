using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.CQRS.Commands.AlbumMedias;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IAlbumMediaRepository : IBaseRepository<AlbumMedia>
{
    Task<AlbumMedia?> GetById(AlbumMediaDeleteCommand command);
}