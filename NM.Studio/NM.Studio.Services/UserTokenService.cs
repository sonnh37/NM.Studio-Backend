using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.UserTokens;
using NM.Studio.Domain.Models.Options;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class UserTokenService : BaseService, IUserTokenService
{
    private readonly string _clientId;
    private readonly IConfiguration _configuration;
    private readonly int _expAccessToken;
    private readonly int _expRefreshToken;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IMapper _mapper;
    private readonly IUserTokenRepository _userTokenRepository;
    protected readonly UserJwtOptions _userJwtOptions;
    protected readonly IUnitOfWork _unitOfWork;

    public UserTokenService(IMapper mapper,
        IUnitOfWork unitOfWork,
        IUserService userService,
        IOptions<UserJwtOptions> userJwtOptions
    ) : base(mapper, unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _httpContextAccessor ??= new HttpContextAccessor();
        _userJwtOptions = userJwtOptions.Value;
        _userTokenRepository = _unitOfWork.UserTokenRepository;
    }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        UserToken? entity = null;
        if (createOrUpdateCommand is UserTokenUpdateCommand updateCommand)
        {
            entity = await _userTokenRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();
            if (entity == null) throw new Exception();
            _mapper.Map(updateCommand, entity);
            _userTokenRepository.Update(entity);
        }
        else if (createOrUpdateCommand is UserTokenCreateCommand createCommand)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            createCommand.UserAgent = httpContext.Request.Headers["User-Agent"].ToString() ?? "Unknown";
            createCommand.IpAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                      ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            createCommand.IpAddress = NormalizeIpAddress(createCommand.IpAddress);
            createCommand.ExpiryTime = DateTimeOffset.UtcNow.AddDays(_userJwtOptions.RefreshTokenExpiryInDays);
            entity = _mapper.Map<UserToken>(createCommand);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _userTokenRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<UserTokenResult>(entity);

        return new BusinessResult(result);
    }

    public BusinessResult ValidateRefreshTokenIpMatch()
    {
        var ipAddress = _httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                        ?? _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];

        ipAddress = NormalizeIpAddress(ipAddress);
        // Kiểm tra refreshToken và IP address
        var storedRefreshToken = _userTokenRepository.GetByRefreshTokenAsync(refreshToken).Result;

        if (storedRefreshToken == null || storedRefreshToken.ExpiryTime < DateTimeOffset.UtcNow)
            throw new DomainException("Your session has expired. Please log in again.");

        if (storedRefreshToken.IpAddress != ipAddress)
            throw new DomainException("Warning!! someone trying to get token.");

        var refreshTokenResult = _mapper.Map<UserTokenResult>(storedRefreshToken);
        return new BusinessResult(refreshTokenResult);
    }

    private string NormalizeIpAddress(string ipAddress)
    {
        if (ipAddress.Contains(",")) ipAddress = ipAddress.Split(',')[0].Trim();

        if (IPAddress.TryParse(ipAddress, out var ip))
        {
            if (ip.IsIPv4MappedToIPv6) return ip.MapToIPv4().ToString();

            // Chuyển loopback IPv6 (::1) về loopback IPv4 (127.0.0.1)
            if (IPAddress.IPv6Loopback.Equals(ip)) return IPAddress.Loopback.ToString(); // Trả về 127.0.0.1
        }

        return ipAddress;
    }
}