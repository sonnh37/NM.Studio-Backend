using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.CQRS.Queries.Albums;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Utilities.Filters;

namespace NM.Studio.Data.Repositories;

public class AlbumRepository : BaseRepository<Album>, IAlbumRepository
{
    public AlbumRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(List<Album>, int)> GetAll(AlbumGetAllQuery query)
    {
        var queryable = GetQueryable();
        
        if (!string.IsNullOrEmpty(query.Title))
            queryable = queryable.Where(m => m.Title!.ToLower().Trim().Contains(query.Title.ToLower().Trim()));

        if (!string.IsNullOrEmpty(query.Description))
            queryable = queryable.Where(m => m.Description!.ToLower().Contains(query.Description.ToLower()));

        if (!string.IsNullOrEmpty(query.Slug))
            queryable = queryable.Where(m => m.Slug!.ToLower().Trim() == query.Slug.ToLower().Trim());

        queryable = BaseFilterHelper.Base(queryable, query);
        
        queryable = Include(queryable, query.IncludeProperties);
        queryable = Sort(queryable, query);

        var totalCount = queryable.Count();
        queryable = query.Pagination.IsPagingEnabled ? GetQueryablePagination(queryable, query) : queryable;

        return (await queryable.ToListAsync(), totalCount);
    }
}