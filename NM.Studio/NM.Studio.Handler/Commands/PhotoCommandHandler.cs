using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.MediaFiles;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class MediaFileCommandHandler : BaseCommandHandler,
    IRequestHandler<MediaFileUpdateCommand, BusinessResult>,
    IRequestHandler<MediaFileDeleteCommand, BusinessResult>,
    IRequestHandler<MediaFileCreateCommand, BusinessResult>
{
    private readonly IMediaFileService _mediaFileService;

    public MediaFileCommandHandler(IMediaFileService mediaFileService) : base(mediaFileService)
    {
        _mediaFileService = mediaFileService;
    }

    public async Task<BusinessResult> Handle(MediaFileCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _mediaFileService.CreateOrUpdate<MediaFileResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(MediaFileDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.DeleteById(request.Id, request.IsPermanent);
        return msgView;
    }
    
    public async Task<BusinessResult> Handle(MediaFileUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.CreateOrUpdate<MediaFileResult>(request);
        return msgView;
    }
}