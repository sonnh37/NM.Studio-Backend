using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;

namespace NM.Studio.Data.Repositories;

public class ProductXPhotoRepository : BaseRepository<ProductXPhoto>, IProductXPhotoRepository
{
    public ProductXPhotoRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public virtual async Task<ProductXPhoto?> GetById(ProductXPhotoDeleteCommand command)
    {
        var queryable = GetQueryable(x => x.ProductId == command.ProductId && x.PhotoId == command.PhotoId);
        var entity = await queryable.FirstOrDefaultAsync();

        return entity;
    }

    public new void Delete(ProductXPhoto entity)
    {
        DbSet.Remove(entity);
    }
}