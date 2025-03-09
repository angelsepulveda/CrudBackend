using Memberships.Submodules.Users.Contracts.Services;

namespace Memberships.Submodules.Users.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IGetByIdUserService, GetByIdUserService>();
        services.AddScoped<IGetByRutUserService, GetByRutUserService>();

        services.AddSingleton<IGenerateTokenService>(
            provider =>
                new GenerateJwtTokenService(
                    configuration["Jwt:Issuer"]
                        ?? throw new ArgumentNullException(nameof(configuration)),
                    configuration["Jwt:Audience"]
                        ?? throw new ArgumentNullException(nameof(configuration)),
                    configuration["Jwt:SecretKey"]
                        ?? throw new ArgumentNullException(nameof(configuration))
                )
        );
        services.AddSingleton<IValidateTokenService, ValidateGoogleTokenService>();

        return services;
    }
}
