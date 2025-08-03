using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IAlbumService : IBaseService
{
    Task<BusinessResult> Create<TResult>(AlbumCreateCommand createCommand) where TResult : BaseResult;
    Task<BusinessResult> Update<TResult>(AlbumUpdateCommand createCommand) where TResult : BaseResult;
}