using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Exceptions;

namespace Memberships.Submodules.Users.Services;

public class GetByRutUserService(MembershipDbContext dbContext) : IGetByRutUserService
{
    public async Task<User?> HandleAsync(string rut)
    {
        User? user = await dbContext.Users.Where(x => x.Rut == rut).FirstOrDefaultAsync();

        if (user is not null && user.Enable == false)
            throw new UserRutAlreadyEnabledException(rut);

        return user;
    }
}
