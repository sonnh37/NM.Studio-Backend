using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.CQRS.Queries.SubCategories;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Utilities.Filters;

namespace NM.Studio.Data.Repositories;

public class SubCategoryRepository : BaseRepository<SubCategory>, ISubCategoryRepository
{
    public SubCategoryRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<(List<SubCategory>, int)> GetAll(SubCategoryGetAllQuery query)
    {
        var queryable = GetQueryable();
        if (query.CategoryId != null)
            queryable = queryable.Where(m => m.CategoryId != query.CategoryId);

        if (query.IsNullCategoryId.HasValue && query.IsNullCategoryId.Value)
            queryable = queryable.Where(m => m.CategoryId == null);

        queryable = Include(queryable, query.IncludeProperties);
        queryable = Sort(queryable, query);

        var totalCount = queryable.Count();
        queryable = query.Pagination.IsPagingEnabled ? GetQueryablePagination(queryable, query) : queryable;

        return (await queryable.ToListAsync(), totalCount);
    }
}