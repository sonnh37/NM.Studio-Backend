using Microsoft.AspNetCore.Mvc;

namespace NM.Studio.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected BaseController()
    {
    }
}