using AutoMapper;
using Microsoft.AspNetCore.Http;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class SizeService : BaseService<Size>, ISizeService
{
    private readonly ISizeRepository _sizeRepository;

    public SizeService(IMapper mapper,
        IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        : base(mapper, unitOfWork, httpContextAccessor)
    {
        _sizeRepository = _unitOfWork.SizeRepository;
    }
}