using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Data.Repositories;

public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken)
    {
        var queryable = GetQueryable(x => x.Token != null && x.Token.ToLower() == refreshToken.ToLower());
        var entity = await queryable.SingleOrDefaultAsync();

        return entity;
    }

    public async Task CleanupExpiredTokensAsync()
    {
        try
        {
            var queryable = GetQueryable(token => token.Expiry < DateTime.UtcNow);
            if (!queryable.Any())
            {
              return;  
            }
            
            var expiredTokens = await queryable.ToListAsync();

            if (expiredTokens.Count != 0)
            {
                DeleteRangePermanently(expiredTokens);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while cleaning expired tokens: {ex.Message}");
            throw;
        }
    }
}