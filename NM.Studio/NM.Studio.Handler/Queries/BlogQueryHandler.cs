using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Blogs;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Handler.Queries;

public class BlogQueryHandler :
    IRequestHandler<BlogGetAllQuery, BusinessResult>,
    IRequestHandler<BlogGetByIdQuery, BusinessResult>
{
    protected readonly IBlogService _blogService;

    public BlogQueryHandler(IBlogService blogService)
    {
        _blogService = blogService;
    }

    public async Task<BusinessResult> Handle(BlogGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _blogService.GetAll<BlogResult>(request);
    }

    public async Task<BusinessResult> Handle(BlogGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _blogService.GetById<BlogResult>(request.Id);
    }
}