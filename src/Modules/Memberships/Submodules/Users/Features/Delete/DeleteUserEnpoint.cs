namespace Memberships.Submodules.Users.Features.Delete;

public class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/api/users/{Id:guid}",
                async (Guid Id, ISender sender) =>
                {
                    Unit result = await sender.Send(new DeleteUserCommand(Id));

                    return Results.Ok(result);
                }
            )
            .WithName("Deleteuser")
            .Produces<Unit>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a User")
            .WithDescription(
                "This endpoint allows you to delete an existing user by providing the user ID in the URL. "
                    + "It returns a status indicating whether the deletion was successful."
            )
            .WithTags("Users");
    }
}
