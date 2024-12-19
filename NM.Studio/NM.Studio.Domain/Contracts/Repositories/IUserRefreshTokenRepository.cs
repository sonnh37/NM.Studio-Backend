using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IUserRefreshTokenRepository : IBaseRepository<UserRefreshToken>
{
    Task<UserRefreshToken?> GetByRefreshTokenAsync(string refreshToken);
}