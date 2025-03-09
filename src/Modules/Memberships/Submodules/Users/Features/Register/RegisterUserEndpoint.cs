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
                    string endpoint = nameof(RegisterRoleEndpoint);
                    Log.Information(
                        "Endpoint: {Endpoint}, payload: {Payload}",
                        endpoint,
                        JsonSerializer.Serialize(payload)
                    );
                    UserDto result = await sender.Send(new RegisterUserCommand(payload));

                    return Results.Ok(result);
                }
            )
            .WithName("RegisterUser")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Registrar un nuevo usuario")
            .WithDescription(
                "Este endpoint permite registrar un nuevo usuario proporcionando los detalles necesarios en el payload. "
                    + "El payload debe incluir el nombre del usuario y otra información requerida. "
                    + "Devuelve los detalles del usuario registrado al ser exitosamente registrado."
            )
            .WithTags("Users");
    }
}
