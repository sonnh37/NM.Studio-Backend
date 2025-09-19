using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories.Base;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Data.Repositories;

public class BlogRepository : BaseRepository<Blog>, IBlogRepository
{
    public BlogRepository(StudioContext dbContext) : base(dbContext)
    {
    }
}