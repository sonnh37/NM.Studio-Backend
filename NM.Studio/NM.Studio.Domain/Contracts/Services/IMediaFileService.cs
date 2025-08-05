using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Queries.MediaFiles;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IMediaFileService : IBaseService
{
    Task<BusinessResult> GetAll(MediaFileGetAllQuery query);

}