using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Carts;
using NM.Studio.Domain.CQRS.Queries.Carts;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class CartController : BaseController
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CartGetAllQuery request)
    {
        var businessResult = await _cartService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] CartGetByIdQuery request)
    {
        var businessResult = await _cartService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartCreateCommand request)
    {
        var businessResult = await _cartService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CartUpdateCommand request)
    {
        var businessResult = await _cartService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] CartDeleteCommand request)
    {
        var businessResult = await  _cartService.Delete(request);

        return Ok(businessResult);
    }
}