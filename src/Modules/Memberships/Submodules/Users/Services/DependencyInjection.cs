using Memberships.Submodules.Users.Contracts.Services;

namespace Memberships.Submodules.Users.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersServices(this IServiceCollection services)
    {
        services.AddScoped<IGetByIdUserService, GetByIdUserService>();
        services.AddScoped<IGetByRutUserService, GetByRutUserService>();

        return services;
    }
}
