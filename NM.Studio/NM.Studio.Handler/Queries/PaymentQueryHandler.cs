using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Payments;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class PaymentQueryHandler :
    IRequestHandler<PaymentGetAllQuery, BusinessResult>,
    IRequestHandler<PaymentGetByIdQuery, BusinessResult>
{
    protected readonly IPaymentService _paymentService;

    public PaymentQueryHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<BusinessResult> Handle(PaymentGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _paymentService.GetAll<PaymentResult>(request);
    }

    public async Task<BusinessResult> Handle(PaymentGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _paymentService.GetById<PaymentResult>(request);
    }
}