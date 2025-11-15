using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.CartItems;
using NM.Studio.Domain.Models.CQRS.Queries.CartItems;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class CartItemController : BaseController
{
    private readonly ICartItemService _cartItemService;

    public CartItemController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CartItemGetAllQuery request)
    {
        var businessResult = await _cartItemService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] CartItemGetByIdQuery request)
    {
        var businessResult = await _cartItemService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartItemCreateCommand request)
    {
        var businessResult = await _cartItemService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CartItemUpdateCommand request)
    {
        var businessResult = await _cartItemService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] CartItemDeleteCommand request)
    {
        var businessResult = await  _cartItemService.Delete(request);

        return Ok(businessResult);
    }
}