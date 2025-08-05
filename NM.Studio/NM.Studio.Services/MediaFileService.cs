using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Queries.MediaFiles;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class MediaFileService : BaseService<MediaFile>, IMediaFileService
{
    private readonly IMediaFileRepository _mediaFileRepository;

    public MediaFileService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _mediaFileRepository = _unitOfWork.MediaFileRepository;
    }
    
    public async Task<BusinessResult> GetAll(MediaFileGetAllQuery query)
    {
        var (entities, totalCount) = await _mediaFileRepository.GetAll(query);
        var results = _mapper.Map<List<MediaFileResult>>(entities);
        var tableResponse = new QueryResult(results, totalCount, query);

        return BusinessResult.Success(tableResponse);
    }
    
}