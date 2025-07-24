using AutoMapper;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Data.Repositories;

public class ProductSizeRepository : BaseRepository<ProductSize>, IProductSizeRepository
{
    public ProductSizeRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}