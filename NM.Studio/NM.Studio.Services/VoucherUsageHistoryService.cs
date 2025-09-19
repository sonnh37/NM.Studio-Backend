using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class VoucherUsageHistoryService : BaseService, IVoucherUsageHistoryService
{
    private readonly IVoucherUsageHistoryRepository _voucherUsageHistoryRepository;

    public VoucherUsageHistoryService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
        _voucherUsageHistoryRepository = _unitOfWork.VoucherUsageHistoryRepository;
    }
}