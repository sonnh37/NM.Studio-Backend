using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.CQRS.Queries.MediaFiles;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Utilities.Filters;

namespace NM.Studio.Data.Repositories;

public class MediaFileRepository : BaseRepository<MediaFile>, IMediaFileRepository
{
    public MediaFileRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
    
    public async Task<(List<MediaFile>, int)> GetAll(MediaFileGetAllQuery query)
    {
        var queryable = GetQueryable();
        
        if (query.IsFeatured.HasValue) queryable = queryable.Where(m => m.IsFeatured == query.IsFeatured);

        if (!string.IsNullOrEmpty(query.Title))
            queryable = queryable.Where(m => m.Title!.ToLower().Trim().Contains(query.Title.ToLower().Trim()));

        if (!string.IsNullOrEmpty(query.Href))
            queryable = queryable.Where(m => m.Href!.ToLower().Trim().Contains(query.Href.ToLower().Trim()));

        if (query.AlbumId != null)
            queryable = queryable.Where(m => !m.AlbumMedias.Select(a => a.AlbumId).Contains(query.AlbumId));

        if (query.ProductId != null)
            queryable = queryable.Where(m => !m.ProductMedias.Select(a => a.ProductId).Contains(query.ProductId));
        
        queryable = BaseFilterHelper.Base(queryable, query);
        queryable = Include(queryable, query.IncludeProperties);
        queryable = Sort(queryable, query);
        

        var totalCount = queryable.Count();
        queryable = query.Pagination.IsPagingEnabled ? GetQueryablePagination(queryable, query) : queryable;

        return (await queryable.ToListAsync(), totalCount);
    }
}