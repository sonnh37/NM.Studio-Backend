using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.SubCategories;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class SubCategoryCommandHandler : BaseCommandHandler,
    IRequestHandler<SubCategoryUpdateCommand, BusinessResult>,
    IRequestHandler<SubCategoryDeleteCommand, BusinessResult>,
    IRequestHandler<SubCategoryCreateCommand, BusinessResult>
{
    protected readonly ISubCategoryService _subCategoryService;

    public SubCategoryCommandHandler(ISubCategoryService subCategoryService) : base(subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }

    public async Task<BusinessResult> Handle(SubCategoryCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _subCategoryService.CreateOrUpdate<SubCategoryResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(SubCategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.DeleteById(request.Id, request.IsPermanent);
        return msgView;
    }

    public async Task<BusinessResult> Handle(SubCategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.CreateOrUpdate<SubCategoryResult>(request);
        return msgView;
    }
}