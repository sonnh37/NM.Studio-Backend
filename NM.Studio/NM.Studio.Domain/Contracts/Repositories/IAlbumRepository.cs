using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.CQRS.Queries.Albums;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IAlbumRepository : IBaseRepository<Album>
{
    Task<(List<Album>, int)> GetAll(AlbumGetAllQuery query);
}