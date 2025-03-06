using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Exceptions;
using Memberships.Submodules.Users.ValueObjects;

namespace Memberships.Submodules.Users.Services;

public class GetByIdUserService(MembershipDbContext dbContext) : IGetByIdUserService
{
    public async Task<User> HandleAsync(UserId id)
    {
        User? user = await dbContext.Users.Where(x => x.Id == id && x.Enable).FirstOrDefaultAsync();

        if (user is null)
            throw new UserNotFoundException();

        return user;
    }
}
