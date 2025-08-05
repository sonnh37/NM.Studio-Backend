using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.VoucherUsageHistories;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class VoucherUsageHistoryCommandHandler :
    IRequestHandler<VoucherUsageHistoryUpdateCommand, BusinessResult>,
    IRequestHandler<VoucherUsageHistoryDeleteCommand, BusinessResult>,
    IRequestHandler<VoucherUsageHistoryCreateCommand, BusinessResult>
{
    protected readonly IVoucherUsageHistoryService _voucherUsageHistoryService;

    public VoucherUsageHistoryCommandHandler(IVoucherUsageHistoryService baseService)
    {
        _voucherUsageHistoryService = baseService;
    }

    public async Task<BusinessResult> Handle(VoucherUsageHistoryCreateCommand request,
        CancellationToken cancellationToken)
    {
        var businessResult = await _voucherUsageHistoryService.CreateOrUpdate<VoucherUsageHistoryResult>(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(VoucherUsageHistoryDeleteCommand request,
        CancellationToken cancellationToken)
    {
        var businessResult = await _voucherUsageHistoryService.Delete(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(VoucherUsageHistoryUpdateCommand request,
        CancellationToken cancellationToken)
    {
        var businessResult = await _voucherUsageHistoryService.CreateOrUpdate<VoucherUsageHistoryResult>(request);
        return businessResult;
    }
}