using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Data.Repositories;

public class UserRefreshTokenRepository : BaseRepository<UserRefreshToken>, IUserRefreshTokenRepository
{
    public UserRefreshTokenRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
    
    public async Task<UserRefreshToken?> GetByRefreshTokenAsync(string refreshToken)
    {
        var queryable = GetQueryable(x => x.RefreshToken != null && x.RefreshToken.ToLower() == refreshToken.ToLower());
        var entity = await queryable.SingleOrDefaultAsync();

        return entity;
    }
}