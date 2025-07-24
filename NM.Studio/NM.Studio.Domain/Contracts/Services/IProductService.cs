using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Queries.Products;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IProductService : IBaseService
{
    Task<BusinessResult> Create<TResult>(ProductCreateCommand createCommand) where TResult : BaseResult;

    Task<BusinessResult> Update<TResult>(ProductUpdateCommand createCommand) where TResult : BaseResult;

    Task<BusinessResult> GetRepresentativeByCategory(ProductRepresentativeByCategoryQuery query);
}