using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Vouchers;
using NM.Studio.Domain.CQRS.Queries.Vouchers;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class VoucherController : BaseController
{
    private readonly IVoucherService _voucherService;

    public VoucherController(IVoucherService voucherService)
    {
        _voucherService = voucherService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] VoucherGetAllQuery request)
    {
        var businessResult = await _voucherService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] VoucherGetByIdQuery request)
    {
        var businessResult = await _voucherService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VoucherCreateCommand request)
    {
        var businessResult = await _voucherService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] VoucherUpdateCommand request)
    {
        var businessResult = await _voucherService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] VoucherDeleteCommand request)
    {
        var businessResult = await  _voucherService.Delete(request);

        return Ok(businessResult);
    }
}