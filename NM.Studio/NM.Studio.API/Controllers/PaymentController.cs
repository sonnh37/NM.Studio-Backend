using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class PaymentController : BaseController
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    
}