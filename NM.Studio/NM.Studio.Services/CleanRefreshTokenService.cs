using Microsoft.Extensions.Logging;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using Quartz;

namespace NM.Studio.Services;

public class CleanRefreshTokenService : IJob
{
    private readonly ILogger<CleanRefreshTokenService> _logger;
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CleanRefreshTokenService(IUserTokenRepository userTokenRepository, IUnitOfWork unitOfWork,
        ILogger<CleanRefreshTokenService> logger)
    {
        _userTokenRepository = userTokenRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation($"Executing CleanRefreshTokenJob at {DateTimeOffset.UtcNow}");
            await _userTokenRepository.CleanupExpiredTokensAsync();
            await _unitOfWork.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CleanRefreshTokenJob");
        }
    }
}