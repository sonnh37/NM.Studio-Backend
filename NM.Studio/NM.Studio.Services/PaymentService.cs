using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class PaymentService : BaseService, IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
        _paymentRepository = _unitOfWork.PaymentRepository;
    }
}