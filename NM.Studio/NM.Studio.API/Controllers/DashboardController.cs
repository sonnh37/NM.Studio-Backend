using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("dashboard")]
public class DashboardController : BaseController
{
    public DashboardController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        // Xử lý logic thống kê theo ngày
        var stats = new
        {
            TotalSales = 500000,
            NewUsers = 300,
            CompletedOrders = 120
        };
        return Ok(stats);
    }
}