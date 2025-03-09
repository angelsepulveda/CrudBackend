namespace Memberships.Submodules.Users.Features.ExportExcel;

public class ExportExcelUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/users/export",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                async (ISender sender) =>
                {
                    MemoryStream files = await sender.Send(new ExportExcelUserQuery());

                    return TypedResults.File(
                        files,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "usuarios.xlsx"
                    );
                }
            )
            .WithName("ExportExcelUser")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Exportar usuarios a Excel")
            .WithDescription(
                "Este endpoint permite exportar una lista de todos los usuarios disponibles en el sistema en formato Excel. "
                    + "Devuelve un archivo Excel con los detalles de los usuarios al ser exitosamente recuperados."
            )
            .WithTags("Users");
    }
}
