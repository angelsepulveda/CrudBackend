namespace Memberships.Submodules.Users.Features.Logout;

public class UserLogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/logout",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (HttpContext http) =>
            {
                await http.SignOutAsync();
                return Results.Ok("Sesión cerrada");
            }
        );
    }
}
