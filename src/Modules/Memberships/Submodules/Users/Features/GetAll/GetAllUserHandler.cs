using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.Dtos;

namespace Memberships.Submodules.Users.Features.GetAll;

public sealed record GetAllUserQuery() : IQuery<List<UserDto>>;

public sealed class GetAllUserHandler(MembershipDbContext dbContext)
    : IQueryHandler<GetAllUserQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(
        GetAllUserQuery request,
        CancellationToken cancellationToken
    )
    {
        List<User> users = await dbContext.Users
            .AsNoTracking()
            .Where(x => x.Enable)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return users
            .Select(
                x =>
                    new UserDto(
                        Id: x.Id.Value,
                        Name: x.Name,
                        Email: x.Email,
                        BirthDate: x.Birthdate,
                        Rut: x.Rut
                    )
            )
            .ToList();
    }
}
