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
            .WithSummary("Eliminar un usuario")
            .WithDescription(
                "Este endpoint permite eliminar un usuario existente proporcionando el ID del usuario en la URL. "
                    + "Devuelve un estado que indica si la eliminación fue exitosa."
            )
            .WithTags("Users");
    }
}
