using AutoMapper;
using Microsoft.AspNetCore.Http;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
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
}