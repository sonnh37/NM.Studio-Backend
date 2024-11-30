using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Blogs;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IBlogService : IBaseService
{
    Task<BusinessResult> Create<TResult>(BlogCreateCommand createCommand) where TResult : BaseResult;
    Task<BusinessResult> Update<TResult>(BlogUpdateCommand createCommand) where TResult : BaseResult;
}