using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Blogs;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

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
        return await _blogService.GetListByQueryAsync<BlogResult>(request);
    }

    public async Task<BusinessResult> Handle(BlogGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _blogService.GetById<BlogResult>(request);
    }
}