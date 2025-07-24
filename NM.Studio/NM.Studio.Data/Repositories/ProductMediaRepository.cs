using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.CQRS.Commands.ProductMedias;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Data.Repositories;

public class ProductMediaRepository : BaseRepository<ProductMedia>, IProductMediaRepository
{
    public ProductMediaRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public virtual async Task<ProductMedia?> GetById(ProductMediaDeleteCommand command)
    {
        var queryable = GetQueryable(x => x.ProductId == command.ProductId && x.MediaFileId == command.MediaFileId);
        var entity = await queryable.FirstOrDefaultAsync();

        return entity;
    }

    public new void Delete(ProductMedia entity)
    {
        DbSet.Remove(entity);
    }
}