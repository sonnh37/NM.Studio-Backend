using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.SubCategories;
using NM.Studio.Domain.Models.CQRS.Queries.SubCategories;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface ISubCategoryService : IBaseService
{
    Task<BusinessResult> GetAll(SubCategoryGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(SubCategoryGetByIdQuery request);
    Task<BusinessResult> Delete(SubCategoryDeleteCommand command);
}