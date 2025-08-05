using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.CQRS.Queries.MediaFiles;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IMediaFileRepository : IBaseRepository<MediaFile>
{
    Task<(List<MediaFile>, int)> GetAll(MediaFileGetAllQuery query);
}