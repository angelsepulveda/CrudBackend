using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Extensions;

public static class AuthenticateExtensions
{
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId =
                    configuration["Google:ClientId"]
                    ?? throw new ArgumentNullException(nameof(configuration));
                googleOptions.ClientSecret =
                    configuration["Google:ClientSecret"]
                    ?? throw new ArgumentNullException(nameof(configuration));
                googleOptions.CallbackPath = "/signin-google";
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer =
                        configuration["Jwt:Issuer"]
                        ?? throw new ArgumentNullException(nameof(configuration)),
                    ValidAudience =
                        configuration["Jwt:Audience"]
                        ?? throw new ArgumentNullException(nameof(configuration)),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])
                    )
                };
            });
        return services;
    }
}
