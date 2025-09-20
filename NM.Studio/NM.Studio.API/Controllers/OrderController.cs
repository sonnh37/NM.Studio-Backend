using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.Orders;
using NM.Studio.Domain.Models.CQRS.Queries.Orders;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class OrderController : BaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] OrderGetAllQuery request)
    {
        var businessResult = await _orderService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] OrderGetByIdQuery request)
    {
        var businessResult = await _orderService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateCommand request)
    {
        var businessResult = await _orderService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] OrderUpdateCommand request)
    {
        var businessResult = await _orderService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] OrderDeleteCommand request)
    {
        var businessResult = await  _orderService.Delete(request);

        return Ok(businessResult);
    }
}