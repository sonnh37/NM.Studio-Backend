using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.CQRS.Commands.AlbumMedias;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Data.Repositories;

public class AlbumMediaRepository : BaseRepository<AlbumMedia>, IAlbumMediaRepository
{
    public AlbumMediaRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }


    public virtual async Task<AlbumMedia?> GetById(AlbumMediaDeleteCommand command)
    {
        var queryable = GetQueryable(x => x.AlbumId == command.AlbumId && x.MediaFileId == command.MediaFileId);
        var entity = await queryable.FirstOrDefaultAsync();

        return entity;
    }

    public new void Delete(AlbumMedia entity)
    {
        DbSet.Remove(entity);
    }
}