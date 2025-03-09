namespace Memberships.Submodules.Users.Features.Authenticate;

public class UserAuthenticateEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/login-google",
            async (ISender sender, [FromBody] TokenRequest request) =>
            {
                string jwtToken = await sender.Send(new UserAuthenticateQuery(request.Token));

                return Results.Ok(new { token = jwtToken });
            }
        );
    }
}
