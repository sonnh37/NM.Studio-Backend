using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.CQRS.Queries.Blogs;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IBlogRepository : IBaseRepository<Blog>
{
    Task<(List<Blog>, int)> GetAll(BlogGetAllQuery query);

}