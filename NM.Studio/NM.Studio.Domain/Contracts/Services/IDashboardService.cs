using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IDashboardService
{
    Task<BusinessResult> GetStats(DashboardGetStatsQuery query);
}