using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;
using OtpNet;

namespace NM.Studio.Services;

public class UserService : BaseService<User>, IUserService
{
    private readonly string _clientId;
    private readonly IConfiguration _configuration;
    private readonly int _expirationMinutes;
    private readonly Dictionary<string, DateTimeOffset> _expiryStorage = new();
    private readonly Dictionary<string, string> _otpStorage = new();
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;

    public UserService(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration)
        : base(mapper, unitOfWork)
    {
        _userRepository = _unitOfWork.UserRepository;
        _refreshTokenRepository = _unitOfWork.RefreshTokenRepository;
        _configuration = configuration;
        _expirationMinutes = int.Parse(_configuration["TokenSetting:AccessTokenExpiryMinutes"] ?? "30");
    }

    public async Task<BusinessResult> UpdatePassword(UserPasswordCommand userPasswordCommand)
    {
        try
        {
            var userCurrent = GetUser();
            if (userCurrent == null) return BusinessResult.Fail("Please, login again.");
            userCurrent.Password = userPasswordCommand.Password;
            SetBaseEntityProperties(userCurrent, EntityOperation.Update);
            _userRepository.Update(userCurrent);
            var isSave = await _unitOfWork.SaveChanges();

            if (!await _unitOfWork.SaveChanges()) return BusinessResult.Fail();

            return BusinessResult.Success();
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }

    public async Task<BusinessResult> Create(UserCreateCommand createCommand)
    {
        try
        {
            var entity = await CreateOrUpdateEntity(createCommand);
            var userResult = _mapper.Map<UserResult>(entity);

            return userResult == null ? BusinessResult.Fail() : BusinessResult.Success(userResult);
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }

    public async Task<BusinessResult> UpdateUserCacheAsync(UserUpdateCacheCommand newCacheJson)
    {
        var userId = GetUser()?.Id;
        if (userId == null)
            return BusinessResult.Fail("Not found");

        var user = await _userRepository.GetQueryable(m => m.Id == userId).SingleOrDefaultAsync();
        if (user == null) return BusinessResult.Fail("No user found.");

        JObject existingCache;
        try
        {
            existingCache = string.IsNullOrWhiteSpace(user.Cache) ? new JObject() : JObject.Parse(user.Cache);
        }
        catch (JsonReaderException ex)
        {
            Console.WriteLine($"Error parsing cache: {ex.Message}, Raw Data: {user.Cache}");
            existingCache = new JObject(); // Nếu lỗi thì dùng cache mới
        }

        if (newCacheJson.Cache != null)
        {
            JObject newCache = JObject.Parse(newCacheJson.Cache);

            existingCache.Merge(newCache, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });
        }

        user.Cache = existingCache.ToString();
        SetBaseEntityProperties(user, EntityOperation.Update);
        ;
        _userRepository.Update(user);
        var isSaveChanges = await _unitOfWork.SaveChanges();
        if (!isSaveChanges)
            return BusinessResult.Fail(Const.FAIL_SAVE_MSG);

        return BusinessResult.Success();
    }


    public async Task<BusinessResult> Update(UserUpdateCommand updateCommand)
    {
        try
        {
            var entity = await CreateOrUpdateEntity(updateCommand);
            var userResult = _mapper.Map<UserResult>(entity);

            return userResult == null ? BusinessResult.Fail() : BusinessResult.Success(userResult);
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }

    private string GenerateSecretKey(int length)
    {
        var secretKey = KeyGeneration.GenerateRandomKey(length);
        return Base32Encoding.ToString(secretKey);
    }

    private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { _clientId }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
        return payload;
    }

    #region Business

    public BusinessResult SendEmail(string email)
    {
        try
        {
            var secret = GenerateSecretKey(10); // Bạn có thể chọn độ dài hợp lý, ví dụ: 10
            var otp = GenerateOTP(secret); // Tạo OTP

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
                       IsBodyHtml = false
                   })
            {
                smtp.Send(message);
                _otpStorage[email] = otp; // Lưu trữ OTP cho email
                _expiryStorage[email] = DateTimeOffset.UtcNow.AddMinutes(5); // OTP hết hạn sau 5 phút
                return BusinessResult.Success();
            }
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
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
        if (_otpStorage.TryGetValue(email, out var storedOtp) &&
            _expiryStorage.TryGetValue(email, out var expiry))
            if (expiry > DateTimeOffset.UtcNow && storedOtp == otpInput)
                return BusinessResult.Fail("OTP validation failed");

        return BusinessResult.Success();
    }


    public async Task<BusinessResult> GetByUsername(string username)
    {
        var user = await _userRepository.GetByUsername(username);

        var userResult = _mapper.Map<UserResult>(user);

        if (userResult == null) return BusinessResult.Fail("User not found");

        return BusinessResult.Success(userResult);
    }

    public BusinessResult DecodeToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        // Kiểm tra nếu token không hợp lệ
        if (!handler.CanReadToken(token)) throw new ArgumentException("Token không hợp lệ", nameof(token));

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
            Exp = long.Parse(exp)
        };

        return BusinessResult.Success(decodedToken);
    }

    // private (string token, string expiration) CreateToken(UserResult user)
    // {
    //     var claims = new List<Claim>
    //     {
    //         new Claim("Id", user.Id.ToString()),
    //         new Claim("Role", user.Role.ToString()),
    //         new Claim("Expiration",
    //             new DateTimeOffset(DateTimeOffset.Now.AddMinutes(_expirationMinutes)).ToUnixTimeSeconds().ToString())
    //     };
    //
    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
    //         _configuration.GetSection("AppSettings:Token").Value!));
    //
    //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
    //
    //
    //     var token = new JwtSecurityToken(
    //         claims: claims,
    //         expires: DateTimeOffset.Now.AddMinutes(_expirationMinutes),
    //         signingCredentials: creds
    //     );
    //
    //     var jwt = new JwtSecurityTokenHandler().WriteToken(token);
    //
    //     return (jwt, DateTimeOffset.Now.AddMinutes(_expirationMinutes).ToString("o"));
    // }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }

    public async Task<BusinessResult> AddUser(UserCreateCommand user)
    {
        var username = await _userRepository.FindUsernameOrEmail(user.Username);
        return username switch
        {
            null => await CreateOrUpdate<UserResult>(user),
            _ => BusinessResult.Fail("The account is already registered.")
        };
    }

    public async Task<BusinessResult> GetByUsernameOrEmail(string key)
    {
        var user = await _userRepository.FindUsernameOrEmail(key);

        var userResult = _mapper.Map<UserLoginResult>(user);

        return userResult switch
        {
            null => BusinessResult.Fail(),
            _ => BusinessResult.Success(userResult)
        };
    }

    public async Task<BusinessResult> GetByRefreshToken(UserGetByRefreshTokenQuery request)
    {
        if (request.RefreshToken == null) return BusinessResult.Fail("Refresh token is null");
        var userRefreshToken = await _refreshTokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
        var refreshTokenResult = _mapper.Map<RefreshTokenResult>(userRefreshToken);

        if (refreshTokenResult == null) return BusinessResult.Fail("Not found refresh token");

        return BusinessResult.Success(refreshTokenResult);
    }

    #endregion

    // public async Task<BusinessResult> VerifyGoogleTokenAsync(VerifyGoogleTokenRequest request)
    // {
    //     var payload = await VerifyGoogleToken(request.Token!);
    //
    //     return payload switch
    //     {
    //         null => new ResponseBuilder()
    //             .WithStatus(Const.FAIL_CODE)
    //             .WithMessage("Invalid Google Token")
    //             .Build(),
    //         _ => new ResponseBuilder<GoogleJsonWebSignature.Payload>()
    //             .WithData(payload)
    //             .WithStatus(Const.SUCCESS_CODE)
    //             .WithMessage("Validate Google Token")
    //             .Build(),
    //     };
    // }


    // public async Task<BusinessResult> LoginByGoogleTokenAsync(VerifyGoogleTokenRequest request)
    // {
    //     var response = VerifyGoogleTokenAsync(request).Result;
    //
    //     if (response.Status != Const.SUCCESS_CODE)
    //     {
    //         return response;
    //     }
    //
    //     var payload = response.Data as GoogleJsonWebSignature.Payload;
    //     var user = await _userRepository.GetByEmail(payload.Email);
    //
    //     if (user == null)
    //     {
    //         return new BusinessResult(Const.FAIL_CODE, Const.NOT_FOUND_USER_LOGIN_BY_GOOGLE_MSG);
    //     }
    //
    //     var userResult = _mapper.Map<UserResult>(user);
    //     var (token, expiration) = CreateToken(userResult);
    //     var loginResponse = new LoginResponse(token, expiration);
    //
    //     return new ResponseBuilder<LoginResponse>()
    //         .WithData(loginResponse)
    //         .WithStatus(Const.SUCCESS_CODE)
    //         .WithMessage(Const.SUCCESS_LOGIN_MSG)
    //         .Build();
    // }
    //
    // public async Task<BusinessResult> FindAccountRegisteredByGoogle(VerifyGoogleTokenRequest request)
    // {
    //     var verifyGoogleToken = new VerifyGoogleTokenRequest
    //     {
    //         Token = request.Token
    //     };
    //
    //     var response = VerifyGoogleTokenAsync(verifyGoogleToken).Result;
    //
    //     if (response.Status != Const.SUCCESS_CODE)
    //     {
    //         return response;
    //     }
    //
    //     var payload = response.Data as GoogleJsonWebSignature.Payload;
    //     var user = await _userRepository.GetByEmail(payload.Email);
    //     var userResult = _mapper.Map<UserResult>(user);
    //     if (userResult == null)
    //     {
    //         return new BusinessResult(Const.SUCCESS_CODE, "Email has not registered by google", null);
    //     }
    //
    //     return new BusinessResult(Const.SUCCESS_CODE, "Email has registered by google", userResult);
    // }
    //
    // public async Task<BusinessResult> RegisterByGoogleAsync(UserCreateByGoogleTokenCommand request)
    // {
    //     var verifyGoogleToken = new VerifyGoogleTokenRequest
    //     {
    //         Token = request.Token
    //     };
    //
    //     var response = VerifyGoogleTokenAsync(verifyGoogleToken).Result;
    //
    //     if (response.Status != Const.SUCCESS_CODE)
    //     {
    //         return response;
    //     }
    //
    //     var payload = response.Data as GoogleJsonWebSignature.Payload;
    //     var user = await _userRepository.GetByEmail(payload.Email);
    //
    //     if (user != null)
    //     {
    //         return new BusinessResult(Const.FAIL_CODE, "Email has existed in server");
    //     }
    //
    //     //string base64Image = await GetBase64ImageFromUrl(payload.Picture);
    //     var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
    //     UserCreateCommand _user = new UserCreateCommand
    //     {
    //         Username = payload.Subject,
    //         Email = payload.Email,
    //         Password = passwordHash,
    //         FirstName = payload.GivenName,
    //         LastName = payload.FamilyName,
    //         Role = Role.Customer,
    //         Avatar = payload.Picture
    //     };
    //
    //     var _response = await AddUser(_user);
    //     var _userAdded = _response.Data as User;
    //     var userResult = _mapper.Map<UserResult>(_userAdded);
    //     var (token, expiration) = CreateToken(userResult);
    //     var loginResponse = new LoginResponse(token, expiration);
    //
    //     return new ResponseBuilder<LoginResponse>()
    //         .WithData(loginResponse)
    //         .WithStatus(Const.SUCCESS_CODE)
    //         .WithMessage(Const.SUCCESS_LOGIN_MSG)
    //         .Build();
    // }
}