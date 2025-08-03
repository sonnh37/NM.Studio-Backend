using MediatR;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Queries.Products;

public class ProductRepresentativeByCategoryQuery : BaseQuery, IRequest<BusinessResult>
{
}