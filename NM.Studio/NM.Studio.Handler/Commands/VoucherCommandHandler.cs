using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Vouchers;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class VoucherCommandHandler :
    IRequestHandler<VoucherUpdateCommand, BusinessResult>,
    IRequestHandler<VoucherDeleteCommand, BusinessResult>,
    IRequestHandler<VoucherCreateCommand, BusinessResult>
{
    protected readonly IVoucherService _voucherService;

    public VoucherCommandHandler(IVoucherService baseService)
    {
        _voucherService = baseService;
    }

    public async Task<BusinessResult> Handle(VoucherCreateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _voucherService.CreateOrUpdate<VoucherResult>(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(VoucherDeleteCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _voucherService.Delete(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(VoucherUpdateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _voucherService.CreateOrUpdate<VoucherResult>(request);
        return businessResult;
    }
}