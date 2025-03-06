using Memberships.Submodules.Users.Services;

namespace Memberships.Submodules.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddSubModuleUsers(this IServiceCollection services)
    {
        services.AddUsersServices();

        return services;
    }
}
