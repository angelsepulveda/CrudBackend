namespace Memberships.Submodules.Users.Features.Update;

public class UpdateRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/users",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            .WithSummary("Actualizar un usuario")
            .WithDescription(
                "Actualiza los detalles de un usuario existente basado en el payload proporcionado. El payload debe incluir el ID del usuario y el nuevo nombre, etc."
            )
            .WithTags("Users");
    }
}
