using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
    Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken);
    Task<RefreshToken?> GetByUserIdAndKeyIdAsync(Guid userId,string kid);
    Task CleanupExpiredTokensAsync();
}