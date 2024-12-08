using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Blogs;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class BlogCommandHandler : BaseCommandHandler,
    IRequestHandler<BlogUpdateCommand, BusinessResult>,
    IRequestHandler<BlogDeleteCommand, BusinessResult>,
    IRequestHandler<BlogCreateCommand, BusinessResult>
{
    protected readonly IBlogService _blogService;

    public BlogCommandHandler(IBlogService blogService) : base(blogService)
    {
        _blogService = blogService;
    }

    public async Task<BusinessResult> Handle(BlogCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _blogService.Create<BlogResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(BlogDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.DeleteById(request.Id, request.IsPermanent);
        return msgView;
    }

    public async Task<BusinessResult> Handle(BlogUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _blogService.Update<BlogResult>(request);
        return msgView;
    }
}