using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Payments;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class PaymentCommandHandler :
    IRequestHandler<PaymentUpdateCommand, BusinessResult>,
    IRequestHandler<PaymentDeleteCommand, BusinessResult>,
    IRequestHandler<PaymentCreateCommand, BusinessResult>
{
    protected readonly IPaymentService _paymentService;

    public PaymentCommandHandler(IPaymentService baseService)
    {
        _paymentService = baseService;
    }

    public async Task<BusinessResult> Handle(PaymentCreateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _paymentService.CreateOrUpdate<PaymentResult>(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(PaymentDeleteCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _paymentService.Delete(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(PaymentUpdateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _paymentService.CreateOrUpdate<PaymentResult>(request);
        return businessResult;
    }
}