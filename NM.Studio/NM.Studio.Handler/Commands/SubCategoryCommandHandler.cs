using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.SubCategories;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class SubCategoryCommandHandler :
    IRequestHandler<SubCategoryUpdateCommand, BusinessResult>,
    IRequestHandler<SubCategoryDeleteCommand, BusinessResult>,
    IRequestHandler<SubCategoryCreateCommand, BusinessResult>
{
    protected readonly ISubCategoryService _subCategoryService;

    public SubCategoryCommandHandler(ISubCategoryService subCategoryService)
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
        var msgView = await _subCategoryService.DeleteById(request.Id, request.IsPermanent);
        return msgView;
    }


    public async Task<BusinessResult> Handle(SubCategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _subCategoryService.CreateOrUpdate<SubCategoryResult>(request);
        return msgView;
    }
}