using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.ValueObjects;

namespace Memberships.Submodules.Users.Contracts.Services;

public interface IGetByIdUserService
{
    Task<User> HandleAsync(UserId id);
}
