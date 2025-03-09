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
            .WithSummary("Obtener todos los usuarios")
            .WithDescription(
                "Este endpoint recupera una lista de todos los usuarios disponibles en el sistema. "
                    + "Devuelve una lista con los detalles de los usuarios al ser exitosamente recuperados."
            )
            .WithTags("Users");
    }
}
