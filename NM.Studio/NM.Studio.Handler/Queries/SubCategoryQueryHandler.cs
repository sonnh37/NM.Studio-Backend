using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.SubCategories;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Handler.Queries;

public class SubCategoryQueryHandler :
    IRequestHandler<SubCategoryGetAllQuery, BusinessResult>,
    IRequestHandler<SubCategoryGetByIdQuery, BusinessResult>
{
    protected readonly ISubCategoryService _subCategoryService;

    public SubCategoryQueryHandler(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }

    public async Task<BusinessResult> Handle(SubCategoryGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _subCategoryService.GetAll<SubCategoryResult>(request);
    }

    public async Task<BusinessResult> Handle(SubCategoryGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _subCategoryService.GetById<SubCategoryResult>(request.Id);
    }
}