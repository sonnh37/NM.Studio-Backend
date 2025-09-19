using Microsoft.EntityFrameworkCore;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Data.Repositories;

public class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
{
    public UserTokenRepository(StudioContext dbContext) : base(dbContext)
    {
    }

    public async Task<UserToken?> GetByRefreshTokenAsync(string refreshToken)
    {
        var queryable = GetQueryable(x => x.Token != null && x.Token.ToLower() == refreshToken.ToLower());
        var entity = await queryable.SingleOrDefaultAsync();

        return entity;
    }

    public async Task<UserToken?> GetByUserIdAndKeyIdAsync(Guid userId, string kid)
    {
        var queryable = GetQueryable(x =>
            x.UserId == userId &&
            x.KeyId != null &&
            x.KeyId.ToLower() == kid.ToLower()
        );

        return await queryable.SingleOrDefaultAsync();
    }

    public async Task CleanupExpiredTokensAsync()
    {
        try
        {
            var queryable = GetQueryable(token => token.Expiry < DateTimeOffset.UtcNow);
            if (!queryable.Any()) return;

            var expiredTokens = await queryable.ToListAsync();

            if (expiredTokens.Count != 0) DeleteRange(expiredTokens, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while cleaning expired tokens: {ex.Message}");
            throw;
        }
    }
}