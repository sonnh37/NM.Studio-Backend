using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.CQRS.Commands.ProductMedias;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IProductMediaRepository : IBaseRepository<ProductMedia>
{
    Task<ProductMedia?> GetById(ProductMediaDeleteCommand command);
    void Delete(ProductMedia entity);
}