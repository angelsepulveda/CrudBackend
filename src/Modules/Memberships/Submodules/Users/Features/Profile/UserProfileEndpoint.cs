namespace Memberships.Submodules.Users.Features.Profile;

public class UserProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "api/auth/profile",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            (HttpContext http) =>
            {
                var user = http.User;

                if (user?.Identity?.IsAuthenticated is null)
                    return Results.Unauthorized();

                string? name = user.Identity.Name;
                string? email = user.FindFirst(ClaimTypes.Email)?.Value;

                return Results.Json(new { name, email });
            }
        );
    }
}
