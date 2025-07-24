using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Categories;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Handler.Queries;

public class CategoryQueryHandler :
    IRequestHandler<CategoryGetAllQuery, BusinessResult>,
    IRequestHandler<CategoryGetByIdQuery, BusinessResult>
{
    protected readonly ICategoryService _categoryService;

    public CategoryQueryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<BusinessResult> Handle(CategoryGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryService.GetListByQueryAsync<CategoryResult>(request);
    }

    public async Task<BusinessResult> Handle(CategoryGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryService.GetById<CategoryResult>(request.Id);
    }
}