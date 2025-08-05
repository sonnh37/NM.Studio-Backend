using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Vouchers;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class VoucherQueryHandler :
    IRequestHandler<VoucherGetAllQuery, BusinessResult>,
    IRequestHandler<VoucherGetByIdQuery, BusinessResult>
{
    protected readonly IVoucherService _voucherService;

    public VoucherQueryHandler(IVoucherService voucherService)
    {
        _voucherService = voucherService;
    }

    public async Task<BusinessResult> Handle(VoucherGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _voucherService.GetAll<VoucherResult>(request);
    }

    public async Task<BusinessResult> Handle(VoucherGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _voucherService.GetById<VoucherResult>(request);
    }
}