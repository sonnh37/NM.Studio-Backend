using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IUserService : IBaseService
{
    Task<BusinessResult> UpdateUserCacheAsync(UserUpdateCacheCommand newCacheJson);
    Task<BusinessResult> UpdatePassword(UserPasswordCommand userPasswordCommand);
    Task<BusinessResult> GetAll(UserGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(UserGetByIdQuery request);
    Task<BusinessResult> Delete(UserDeleteCommand command);
}