using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.ServiceBookings;
using NM.Studio.Domain.CQRS.Queries.ServiceBookings;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class ServiceBookingController : BaseController
{
    private readonly IServiceBookingService _serviceBookingService;

    public ServiceBookingController(IServiceBookingService serviceBookingService)
    {
        _serviceBookingService = serviceBookingService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ServiceBookingGetAllQuery request)
    {
        var businessResult = await _serviceBookingService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] ServiceBookingGetByIdQuery request)
    {
        var businessResult = await _serviceBookingService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServiceBookingCreateCommand request)
    {
        var businessResult = await _serviceBookingService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ServiceBookingUpdateCommand request)
    {
        var businessResult = await _serviceBookingService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ServiceBookingDeleteCommand request)
    {
        var businessResult = await  _serviceBookingService.Delete(request);

        return Ok(businessResult);
    }
    
    [HttpPut("cancel")]
    public async Task<IActionResult> Cancel([FromBody] ServiceBookingCancelCommand command)
    {
        var businessResult = await _serviceBookingService.Cancel(command);

        return Ok(businessResult);
    }
}