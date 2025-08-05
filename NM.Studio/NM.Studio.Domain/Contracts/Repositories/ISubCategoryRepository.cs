using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.CQRS.Queries.SubCategories;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface ISubCategoryRepository : IBaseRepository<SubCategory>

{
    Task<(List<SubCategory>, int)> GetAll(SubCategoryGetAllQuery query);
}