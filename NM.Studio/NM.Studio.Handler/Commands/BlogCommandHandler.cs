using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Blogs;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class BlogCommandHandler : IRequestHandler<BlogUpdateCommand, BusinessResult>,
    IRequestHandler<BlogDeleteCommand, BusinessResult>,
    IRequestHandler<BlogCreateCommand, BusinessResult>
{
    protected readonly IBlogService _blogService;

    public BlogCommandHandler(IBlogService blogService)
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
        var msgView = await _blogService.Delete(request);
        return msgView;
    }


    public async Task<BusinessResult> Handle(BlogUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _blogService.Update<BlogResult>(request);
        return msgView;
    }
}