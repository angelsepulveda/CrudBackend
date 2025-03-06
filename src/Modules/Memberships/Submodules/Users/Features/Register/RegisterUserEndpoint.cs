using Memberships.Submodules.Users.Dtos;

namespace Memberships.Submodules.Users.Features.Register;

public class RegisterRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/users",
                async (RegisterUserPayload payload, ISender sender) =>
                {
                    UserDto result = await sender.Send(new RegisterUserCommand(payload));

                    return Results.Ok(result);
                }
            )
            .WithName("RegisterUser")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Register a new user")
            .WithDescription(
                "This endpoint allows you to register a new ruser by providing the necessary details in the payload. "
                    + "The payload must include the user name and other required information. "
                    + "It returns the registered user details upon successful registration."
            )
            .WithTags("Users");
    }
}
