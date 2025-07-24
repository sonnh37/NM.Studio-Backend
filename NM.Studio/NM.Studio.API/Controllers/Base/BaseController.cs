using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NM.Studio.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]

public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController()
    {
    }

    protected BaseController(IMediator mediator) : this()
    {
        _mediator = mediator;
    }
}