using Memberships.Submodules.Users;
using Shared.Data.Interceptors;

namespace Memberships;

public static class MembershipsModule
{
    public static IServiceCollection AddMembershipModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string connectionString =
            configuration.GetConnectionString(GlobalConstants.DefaultConnectionString)
            ?? throw new ArgumentNullException(nameof(configuration));

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<MembershipDbContext>(
            (sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
            }
        );

        services.AddMembershipSubModules();

        return services;
    }

    public static IApplicationBuilder UseMembershipModule(this IApplicationBuilder app)
    {
        app.UseMigration<MembershipDbContext>();

        return app;
    }

    private static IServiceCollection AddMembershipSubModules(this IServiceCollection services)
    {
        services.AddSubModuleUsers();

        return services;
    }
}
