using Memberships.Submodules.Roles.Entities;

namespace Memberships.Submodules.Users.Contracts.Services;

public interface IGetByRutUserService
{
    Task<User?> HandleAsync(string rut);
}
