using Memberships.Submodules.Users.Dtos;

namespace Memberships.Submodules.Users.Features.GetAll;

public class GetAllUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/users",
                async (ISender sender) =>
                {
                    List<UserDto> roles = await sender.Send(new GetAllUserQuery());

                    return Results.Ok(roles);
                }
            )
            .WithName("GetAllUser")
            .Produces<List<UserDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Retrieve all users")
            .WithDescription(
                "This endpoint retrieves a list of all users available in the system. "
                    + "It returns a list of user details upon successful retrieval."
            )
            .WithTags("Users");
    }
}
