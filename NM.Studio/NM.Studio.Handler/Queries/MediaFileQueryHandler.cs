using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.MediaFiles;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class MediaFileQueryHandler :
    IRequestHandler<MediaFileGetAllQuery, BusinessResult>,
    IRequestHandler<MediaFileGetByIdQuery, BusinessResult>
{
    protected readonly IMediaFileService _mediaFileService;

    public MediaFileQueryHandler(IMediaFileService mediaFileService)
    {
        _mediaFileService = mediaFileService;
    }

    public async Task<BusinessResult> Handle(MediaFileGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _mediaFileService.GetAll<MediaFileResult>(request);
    }

    public async Task<BusinessResult> Handle(MediaFileGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _mediaFileService.GetById<MediaFileResult>(request);
    }
}