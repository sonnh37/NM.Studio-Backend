using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.VoucherUsageHistories;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class VoucherUsageHistoryQueryHandler :
    IRequestHandler<VoucherUsageHistoryGetAllQuery, BusinessResult>,
    IRequestHandler<VoucherUsageHistoryGetByIdQuery, BusinessResult>
{
    protected readonly IVoucherUsageHistoryService _voucherUsageHistoryService;

    public VoucherUsageHistoryQueryHandler(IVoucherUsageHistoryService voucherUsageHistoryService)
    {
        _voucherUsageHistoryService = voucherUsageHistoryService;
    }

    public async Task<BusinessResult> Handle(VoucherUsageHistoryGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _voucherUsageHistoryService.GetAll<VoucherUsageHistoryResult>(request);
    }

    public async Task<BusinessResult> Handle(VoucherUsageHistoryGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _voucherUsageHistoryService.GetById<VoucherUsageHistoryResult>(request);
    }
}