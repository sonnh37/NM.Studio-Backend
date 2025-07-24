using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindUsernameOrEmail(string key);
    Task<User?> GetByEmail(string keyword);
    Task<User?> GetByUsername(string username);
}