using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("users")]
public class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UserGetAllQuery userGetAllQuery)
    {
        var messageResult = await _mediator.Send(userGetAllQuery);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var userGetByIdQuery = new UserGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(userGetByIdQuery);

        return Ok(messageResult);
    }
    
    
    [HttpGet("info")]
    public async Task<IActionResult> GetUser()
    {
        var messageResult = await GetCurrentUser();
        
        return Ok(messageResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateCommand userCreateCommand)
    {
        var messageView = await _mediator.Send(userCreateCommand);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdateCommand userUpdateCommand)
    {
        var messageView = await _mediator.Send(userUpdateCommand);

        return Ok(messageView);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] UserRestoreCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }
    
    [HttpPut("password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordCommand userUpdateCommand)
    {
        var messageView = await _mediator.Send(userUpdateCommand);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] UserDeleteCommand userDeleteCommand)
    {
        var messageView = await _mediator.Send(userDeleteCommand);

        return Ok(messageView);
    }
    
    [HttpGet("account")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAccount([FromQuery] UserGetByAccountQuery request)
    {
        var msg = await _mediator.Send(request);
        return Ok(msg);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthQuery authQuery)
    {
        try
        {
            var messageResult = await _mediator.Send(authQuery);
            if(messageResult.Status != 1) return Ok(messageResult);
            var _object = messageResult.Data as TokenResult;
            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set true khi chạy trên HTTPS
                SameSite = SameSiteMode.None, // Đảm bảo chỉ gửi cookie trong cùng domain
                Expires = DateTime.UtcNow.AddMinutes(30) // AccessToken có thể hết hạn sau 1 giờ
            };

            // Cấu hình cookie cho RefreshToken (thời gian sống dài hơn)
            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set true khi chạy trên HTTPS
                SameSite = SameSiteMode.None, // Đảm bảo chỉ gửi cookie trong cùng domain
                Expires = DateTime.UtcNow.AddDays(7) // RefreshToken có thể hết hạn sau 7 ngày
            };

            // Set cookies vào HttpContext
            Response.Cookies.Append("accessToken", _object.Token, accessTokenOptions);
            Response.Cookies.Append("refreshToken", _object.RefreshToken, refreshTokenOptions);      
            return Ok(ResponseHelper.GetToken(_object.Token, ""));
        }
        catch (Exception ex)
        {
            return Ok(ResponseHelper.Error(ex.Message));
        }
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var userLogoutCommand = new UserLogoutCommand
            {
                RefreshToken = refreshToken
            };
            var messageView = await _mediator.Send(userLogoutCommand);
    
            if(messageView.Status != 1) return Ok(messageView);
            
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Ok(messageView);
        }
        catch (Exception ex)
        {
            return Ok(ResponseHelper.Error(ex.Message));
        }
    }
    
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] UserRefreshTokenCommand request)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        
        request.RefreshToken = refreshToken;
        var messageResult = await _mediator.Send(request);
        if(messageResult.Status != 1) return Ok(messageResult);
        var _object = messageResult.Data as TokenResult;
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Set true khi chạy trên HTTPS
            SameSite = SameSiteMode.None, // Đảm bảo chỉ gửi cookie trong cùng domain
            Expires = DateTime.UtcNow.AddMinutes(30) // AccessToken có thể hết hạn sau 1 giờ
        };

        Response.Cookies.Append("accessToken", _object.Token, accessTokenOptions);
        return Ok(ResponseHelper.GetToken(_object.Token, ""));
    }
    
    [HttpPost("check-access")]
    public IActionResult CheckAccess()
    {
        try
        {
            // Lấy thông tin từ token
            var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var role = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            if (role == Role.Admin.ToString() || role == Role.Staff.ToString())
            {
                return Ok(ResponseHelper.Success("Bạn có quyền truy cập."));
            }

            // Trả về lỗi nếu không có quyền
            return Ok(ResponseHelper.Error("Bạn không có quyền truy cập."));
        }
        catch (Exception ex)
        {
            return Ok(ResponseHelper.Error("Error system."));
        }
    }

    [AllowAnonymous]
    // POST api/<AuthController>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateCommand request)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        request.Password = passwordHash;
        
        var messageView = await _mediator.Send(request);
        return Ok(messageView);
    }
    
    [HttpPost("decode-token")]
   
    public async Task<IActionResult> DecodeToken([FromBody] DecodedTokenQuery request)
    {
        var messageView = await _mediator.Send(request);
        return Ok(messageView);
    }

    [AllowAnonymous]
    [HttpPost("send-email")]
    public async Task<IActionResult> SendOTP([FromBody] UserSendEmailQuery request)
    {
        var messageView = await _mediator.Send(request);
        return Ok(messageView);
    }

    [AllowAnonymous]
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPQuery request)
    {
        var messageView = await _mediator.Send(request);
        return Ok(messageView);
    }



    [AllowAnonymous]
    [HttpPost("find-account-registered-by-google")]
    public async Task<IActionResult> FindAccountRegisteredByGoogle([FromBody] UserGetByGoogleTokenQuery request)
    {
        var messageView = await _mediator.Send(request);
        return Ok(messageView);
    }

    [AllowAnonymous]
    [HttpPost("login-by-google")]
    public async Task<IActionResult> LoginByGoogle([FromBody] AuthByGoogleTokenQuery request)
    {
        var messageView = await _mediator.Send(request);
        return Ok(messageView);
    }

    [AllowAnonymous]
    [HttpPost("register-by-google")]
    public async Task<IActionResult> RegisterByGoogle([FromBody] UserCreateByGoogleTokenCommand request)
    {
        var messageView = await _mediator.Send(request);
        return Ok(messageView);
    }
}