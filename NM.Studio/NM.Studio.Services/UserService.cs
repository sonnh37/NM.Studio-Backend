using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Services.Bases;
using OtpNet;

namespace NM.Studio.Services;

public class UserService : BaseService<User>, IUserService
{
    private readonly IConfiguration configuration;
    private readonly IUserRepository _userRepository;
    private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
    private readonly DateTime _expirationTime = DateTime.Now.AddMinutes(30);
    private readonly Dictionary<string, string> _otpStorage = new(); // Lưu OTP
    private readonly Dictionary<string, DateTime> _expiryStorage = new();
    private readonly string _clientId;

    public UserService(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor)
        : base(mapper, unitOfWork, httpContextAccessor)
    {
        _userRepository = _unitOfWork.UserRepository;
        _userRefreshTokenRepository = _unitOfWork.UserRefreshTokenRepository;
        configuration = _configuration;
    }
    private string GenerateSecretKey(int length)
    {
        byte[] secretKey = KeyGeneration.GenerateRandomKey(length);
        return Base32Encoding.ToString(secretKey);
    }


    #region Business
    public BusinessResult SendEmail(string email)
    {
        try
        {
            string secret = GenerateSecretKey(10); // Bạn có thể chọn độ dài hợp lý, ví dụ: 10
            string otp = GenerateOTP(secret); // Tạo OTP

            var fromAddress = new MailAddress("sonnh1106.se@gmail.com");
            var toAddress = new MailAddress(email);
            const string frompass = "lxnx wdda jepd cxcy"; // Bảo mật tốt hơn là lưu trong biến môi trường
            const string subject = "OTP code";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, frompass),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = otp,
                IsBodyHtml = false,
            })
            {
                smtp.Send(message);
                _otpStorage[email] = otp; // Lưu trữ OTP cho email
                _expiryStorage[email] = DateTime.UtcNow.AddMinutes(5); // OTP hết hạn sau 5 phút
                return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_SAVE_MSG);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return ResponseHelper.Error(ex.Message);
        }
    }

    private string GenerateOTP(string secret)
    {
        var key = Base32Encoding.ToBytes(secret); 
        var totp = new Totp(key);
        return totp.ComputeTotp(); // Tạo OTP
    }

    public BusinessResult ValidateOtp(string email, string otpInput)
    {
        if (_otpStorage.TryGetValue(email, out string storedOtp) && _expiryStorage.TryGetValue(email, out DateTime expiry))
        {
            if (expiry > DateTime.UtcNow && storedOtp == otpInput)
            {
                return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_READ_MSG);
            }
        }
        return new BusinessResult(Const.FAIL_CODE, Const.FAIL_READ_MSG);
    }


    public async Task<BusinessResult> GetByUsername(string username)
    {
        var user = await _userRepository.GetByUsername(username);

        var userResult = _mapper.Map<UserResult>(user);

        if (user != null)
        {
            return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_READ_MSG, userResult);
        }
        else
        {
            return new BusinessResult(Const.FAIL_CODE, "Username khong ton tai", userResult);
        }
    }
    public BusinessResult DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        // Kiểm tra nếu token không hợp lệ
        if (!handler.CanReadToken(token))
        {
            throw new ArgumentException("Token không hợp lệ", nameof(token));
        }

        // Giải mã token
        var jwtToken = handler.ReadJwtToken(token);

        // Truy xuất các thông tin từ token
        var id = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        var name = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        var role = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
        var exp = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Expiration).Value;


        // Tạo đối tượng DecodedToken
        var decodedToken = new DecodedToken
        {
            Id = id,
            Name = name,
            Role = role,
            Exp = long.Parse(exp),
        };

        return new BusinessResult(Const.SUCCESS_CODE, "Decoded to get user", decodedToken);
    }
    private (string token, string expiration) CreateToken(UserResult user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Role", user.Role.ToString()),
            new Claim("Expiration", new DateTimeOffset(_expirationTime).ToUnixTimeSeconds().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


        var token = new JwtSecurityToken(
            claims: claims,
            expires: _expirationTime,
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return (jwt, _expirationTime.ToString("o"));
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }
    public async Task<BusinessResult> Login(AuthQuery query)
    {
        var user = await _userRepository.FindUsernameOrEmail(query.Account);
        var result = _mapper.Map<UserResult>(user);
        //check username 
        if (user == null) 
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, "Not found account", null);

        //check password
        if (!BCrypt.Net.BCrypt.Verify(query.Password, user.Password))
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, "Not match password", null);

        var (token, expiration) = CreateToken(result);

        var refreshToken = GenerateRefreshToken();

        await SaveRefreshToken(user.Id, refreshToken);

        return new BusinessResult(1, "", new TokenResult{ Token = token, RefreshToken = refreshToken });
    }
    
    private async Task SaveRefreshToken(Guid userId, string refreshToken)
    {
        var expirationDate = DateTime.UtcNow.AddMonths(1); // Refresh token expires in 1 month

        var refreshTokenEntity = new UserRefreshToken
        {
            UserId = userId,
            RefreshToken = refreshToken,
            ExpirationDate = expirationDate
        };
        _userRefreshTokenRepository.Add(refreshTokenEntity);
        await _unitOfWork.SaveChanges();
    }

    public async Task<BusinessResult> AddUser(UserCreateCommand user)
    {
        var username = await _userRepository.FindUsernameOrEmail(user.Username);
        return username switch
        {
            null => await base.CreateOrUpdate<UserResult>(user),
            _ => new BusinessResult(Const.FAIL_CODE, "Account existed")
        };
    }

    public async Task<BusinessResult> GetByUsernameOrEmail(string key)
    {
        var user = await _userRepository.FindUsernameOrEmail(key);

        var userResult = _mapper.Map<UserLoginResult>(user);

        return userResult switch
        {
            null => new BusinessResult(Const.FAIL_CODE, Const.NOT_FOUND_MSG, userResult),
            _ => new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_READ_MSG, userResult)
        };
    }

    public async Task<BusinessResult> RefreshToken(UserRefreshTokenCommand request)
    {
        if (request.RefreshToken == null) return ResponseHelper.Warning("Refresh token could not be retrieved");
        var refreshToken = request.RefreshToken;

        // Validate refresh token from request
        var storedRefreshToken = await _userRefreshTokenRepository.GetByRefreshTokenAsync(refreshToken);

        if (storedRefreshToken == null || storedRefreshToken.ExpirationDate < DateTime.UtcNow)
        {
            return ResponseHelper.Warning("Refresh token is expired.");
        }

        // Get user info from the refresh token
        var user = await _userRepository.GetById(storedRefreshToken.UserId);
        var userResult = _mapper.Map<UserResult>(user);

        // Create new access token
        var (token, expiration) = CreateToken(userResult);
        
        return new BusinessResult(1, "", new TokenResult{ Token = token, RefreshToken = refreshToken });
    }

    #endregion

    private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string> { _clientId }
        };

        GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
        return payload;
    }

    public async Task<BusinessResult> VerifyGoogleTokenAsync(VerifyGoogleTokenRequest request)
    {

        var payload = await VerifyGoogleToken(request.Token!);
        
        return payload switch
        {
            null => new BusinessResult(Const.FAIL_CODE, Const.FAIL_READ_GOOGLE_TOKEN_MSG),
            _ => new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_READ_MSG, payload)
        };
    }


    public async Task<BusinessResult> LoginByGoogleTokenAsync(VerifyGoogleTokenRequest request)
    {

        var response = VerifyGoogleTokenAsync(request).Result;

        if (response.Status != Const.SUCCESS_CODE)
        {
            return response;
        }

        var payload = response.Data as GoogleJsonWebSignature.Payload;
        var user = await _userRepository.GetByEmail(payload.Email);

        if (user == null)
        {
            return new BusinessResult(Const.FAIL_CODE, Const.NOT_FOUND_USER_LOGIN_BY_GOOGLE_MSG);
        }

        var userResult = _mapper.Map<UserResult>(user);
        var (token, expiration) = CreateToken(userResult);

        return ResponseHelper.GetToken(token, expiration);
    }

    public async Task<BusinessResult> FindAccountRegisteredByGoogle(VerifyGoogleTokenRequest request)
    {

        var verifyGoogleToken = new VerifyGoogleTokenRequest
        {
            Token = request.Token
        };

        var response = VerifyGoogleTokenAsync(verifyGoogleToken).Result;

        if (response.Status != Const.SUCCESS_CODE)
        {
            return response;
        }

        var payload = response.Data as GoogleJsonWebSignature.Payload;
        var user = await _userRepository.GetByEmail(payload.Email);
        var userResult = _mapper.Map<UserResult>(user);
        if (userResult == null)
        {
            return new BusinessResult(Const.SUCCESS_CODE, "Email has not registered by google", null);
        }

        return new BusinessResult(Const.SUCCESS_CODE, "Email has registered by google", userResult);
    }

    public async Task<BusinessResult> RegisterByGoogleAsync(UserCreateByGoogleTokenCommand request)
    {
        var verifyGoogleToken = new VerifyGoogleTokenRequest
        {
            Token = request.Token
        };

        var response = VerifyGoogleTokenAsync(verifyGoogleToken).Result;

        if (response.Status != Const.SUCCESS_CODE)
        {
            return response;
        }

        var payload = response.Data as GoogleJsonWebSignature.Payload;
        var user = await _userRepository.GetByEmail(payload.Email);

        if (user != null)
        {
            return new BusinessResult(Const.FAIL_CODE, "Email has existed in server");
        }

        //string base64Image = await GetBase64ImageFromUrl(payload.Picture);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        UserCreateCommand _user = new UserCreateCommand
        {
            Username = payload.Subject,
            Email = payload.Email,
            Password = passwordHash,
            FirstName = payload.GivenName,
            LastName = payload.FamilyName,
            Role = Role.Customer,
            ImageUrl = payload.Picture
        };

        var _response = await AddUser(_user);
        var _userAdded = _response.Data as User;
        var userResult = _mapper.Map<UserResult>(_userAdded);
        var (token, expiration) = CreateToken(userResult);

        return ResponseHelper.GetToken(token, expiration);
    }
}