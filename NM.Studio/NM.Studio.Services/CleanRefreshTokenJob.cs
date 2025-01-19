namespace NM.Studio.Services;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using Quartz;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class CleanRefreshTokenJob : IJob
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CleanRefreshTokenJob> _logger;

    public CleanRefreshTokenJob(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ILogger<CleanRefreshTokenJob> logger)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation($"Executing CleanRefreshTokenJob at {DateTime.UtcNow}");
            await _refreshTokenRepository.CleanupExpiredTokensAsync();
            await _unitOfWork.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CleanRefreshTokenJob");
        }
    }
}
