using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.Responses;
using MediatR;
using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class CategoryCommandHandler : BaseCommandHandler,
    IRequestHandler<CategoryUpdateCommand, BusinessResult>,
    IRequestHandler<CategoryDeleteCommand, BusinessResult>,
    IRequestHandler<CategoryCreateCommand, BusinessResult>
{
    protected readonly ICategoryService _categoryService;

    public CategoryCommandHandler(ICategoryService categoryService) : base(categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<BusinessResult> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _categoryService.CreateOrUpdate<CategoryResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.DeleteById(request.Id);
        return msgView;
    }

    public async Task<BusinessResult> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.CreateOrUpdate<CategoryResult>(request);
        return msgView;
    }
}