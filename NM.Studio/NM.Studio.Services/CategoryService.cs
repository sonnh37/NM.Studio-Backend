using AutoMapper;
using Microsoft.AspNetCore.Http;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class CategoryService : BaseService<Category>, ICategoryService
{
    private readonly ICategoryRepository _albumRepository;

    public CategoryService(IMapper mapper,
        IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        : base(mapper, unitOfWork, httpContextAccessor)
    {
        _albumRepository = _unitOfWork.CategoryRepository;
    }
}