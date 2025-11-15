using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IUserTokenRepository : IBaseRepository<UserToken>
{
    Task<UserToken?> GetByRefreshTokenAsync(string refreshToken);
    // Task<UserToken?> GetByUserIdAndKeyIdAsync(Guid userId, string kid);
    Task CleanupExpiredTokensAsync();
}