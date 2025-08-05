using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class SubCategoryService : BaseService<SubCategory>, ISubCategoryService
{
    private readonly ISubCategoryRepository _subCategoryRepository;

    public SubCategoryService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _subCategoryRepository = _unitOfWork.SubCategoryRepository;
    }
    
    public async Task<BusinessResult> GetAll(GetQueryableQuery query)
    {
        var (entities, totalCount) = await _subCategoryRepository.GetAll(query);
        var results = _mapper.Map<List<SubCategoryResult>>(entities);
        var tableResponse = new QueryResult(results, totalCount, query);

        return BusinessResult.Success(tableResponse);
    }
}