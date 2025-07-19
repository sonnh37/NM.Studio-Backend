using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.RefreshTokens;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class RefreshTokenService : BaseService<RefreshToken>, IRefreshTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly int _expAccessToken;
    private readonly int _expRefreshToken;
    private readonly string _clientId;
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    protected readonly TokenSetting _tokenSetting;

    public RefreshTokenService(IMapper mapper,
        IUnitOfWork unitOfWork,
        IUserService userService,
        IOptions<TokenSetting> tokenSetting
    ) : base(mapper, unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _httpContextAccessor ??= new HttpContextAccessor();
        _tokenSetting = tokenSetting.Value;
        _userRepository = _unitOfWork.UserRepository;
        _userService = userService;
        _refreshTokenRepository = _unitOfWork.RefreshTokenRepository;
    }

    public async Task<BusinessResult> CreateOrUpdate<TResult>(CreateOrUpdateCommand createOrUpdateCommand)
        where TResult : BaseResult
    {
        try
        {
            var entity = await CreateOrUpdateEntity(createOrUpdateCommand);
            var result = _mapper.Map<TResult>(entity);
            if (result == null)
                return new ResponseBuilder()
                    .WithStatus(Const.FAIL_CODE)
                    .WithMessage(Const.FAIL_SAVE_MSG).Build();

            var msg = new ResponseBuilder<TResult>()
                .WithData(result)
                .WithStatus(Const.SUCCESS_CODE)
                .WithMessage(Const.SUCCESS_SAVE_MSG)
                .Build();

            return msg;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(RefreshToken).Name}: {ex.Message}";
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(errorMessage)
                .Build();
        }
    }
    private string NormalizeIpAddress(string ipAddress)
    {
        if (ipAddress.Contains(","))
        {
            ipAddress = ipAddress.Split(',')[0].Trim();
        }
        
        if (IPAddress.TryParse(ipAddress, out var ip))
        {
            if (ip.IsIPv4MappedToIPv6)
            {
                return ip.MapToIPv4().ToString();
            }
            
            // Chuyển loopback IPv6 (::1) về loopback IPv4 (127.0.0.1)
            if (IPAddress.IPv6Loopback.Equals(ip))
            {
                return IPAddress.Loopback.ToString(); // Trả về 127.0.0.1
            }
        }
        return ipAddress; 
    }
    public BusinessResult ValidateRefreshTokenIpMatch()
    {
        var ipAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                        ?? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
        
        ipAddress = NormalizeIpAddress(ipAddress);
        // Kiểm tra refreshToken và IP address
        var storedRefreshToken = _refreshTokenRepository.GetByRefreshTokenAsync(refreshToken).Result;

        if (storedRefreshToken == null || storedRefreshToken.Expiry < DateTime.UtcNow)
        {
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage("Your session has expired. Please log in again.")
                .Build();
        }

        if (storedRefreshToken.IpAddress != ipAddress)
        {
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage("Warning!! someone trying to get token.")
                .Build();
        }
        
        var refreshTokenResult = _mapper.Map<RefreshTokenResult>(storedRefreshToken);
        return new ResponseBuilder<RefreshTokenResult>()
            .WithData(refreshTokenResult)
            .WithStatus(Const.SUCCESS_CODE)
            .WithMessage(Const.SUCCESS_READ_MSG)
            .Build();
    }

    protected async Task<RefreshToken?> CreateOrUpdateEntity(CreateOrUpdateCommand createOrUpdateCommand)
    {
        RefreshToken? entity = null;
        if (createOrUpdateCommand is RefreshTokenUpdateCommand updateCommand)
        {
            entity = await _refreshTokenRepository.GetById(updateCommand.Id);
            if (entity == null) return null;

            _mapper.Map(updateCommand, entity);

            InitializeBaseEntityForUpdate(entity);
            _refreshTokenRepository.Update(entity);
        }
        else if (createOrUpdateCommand is RefreshTokenCreateCommand createCommand)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            createCommand.UserAgent = httpContext.Request.Headers["User-Agent"].ToString() ?? "Unknown";
            createCommand.IpAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                      ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            createCommand.IpAddress = NormalizeIpAddress(createCommand.IpAddress);
            createCommand.Expiry = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpiryDays);
            entity = _mapper.Map<RefreshToken>(createCommand);
            if (entity == null) return null;
            entity.Id = Guid.NewGuid();
            InitializeBaseEntityForCreate(entity);
            _refreshTokenRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        return saveChanges ? entity : default;
    }
}