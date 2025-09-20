using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Data.Repositories;

public class ProductMediaRepository : BaseRepository<ProductMedia>, IProductMediaRepository
{
    public ProductMediaRepository(StudioContext dbContext) : base(dbContext)
    {
    }
}