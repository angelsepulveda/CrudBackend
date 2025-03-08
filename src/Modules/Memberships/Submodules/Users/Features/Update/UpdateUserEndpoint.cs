namespace Memberships.Submodules.Users.Features.Update;

public class UpdateRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/users",
                async (UpdateUserPayload payload, ISender sender) =>
                {
                    Log.Information(
                        "Endpoint: {Endpoint}, payload: {Payload}",
                        nameof(UpdateRoleEndpoint),
                        JsonSerializer.Serialize(payload)
                    );

                    Unit result = await sender.Send(new UpdateUserCommand(payload));

                    return Results.Ok(result);
                }
            )
            .WithName("UpdateUser")
            .Produces<Unit>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a User")
            .WithDescription(
                "Updates the details of an existing user based on the provided payload. The payload must include the user ID and the new name, etc."
            )
            .WithTags("Users");
    }
}
