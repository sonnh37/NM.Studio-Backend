using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.CQRS.Commands.ProductXColors;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Data.Repositories;

public class ProductXColorRepository : BaseRepository<ProductXColor>, IProductXColorRepository
{
    public ProductXColorRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

   
}