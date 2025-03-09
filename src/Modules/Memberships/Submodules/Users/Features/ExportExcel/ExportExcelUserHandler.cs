using Memberships.Submodules.Roles.Entities;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;

namespace Memberships.Submodules.Users.Features.ExportExcel;

public class ExportExcelUserDto
{
    [ExcelColumn(Name = "Nombre", Index = 0, Width = 40)]
    public string Name { get; set; } = null!;

    [ExcelColumn(Name = "Rut", Index = 1, Width = 50)]
    public string Rut { get; set; } = null!;

    [ExcelColumn(Name = "Correo electrónico", Index = 2, Width = 60)]
    public string Email { get; set; } = null!;

    [ExcelColumn(Name = "Fecha de Nacimiento", Index = 3, Width = 40)]
    public string BirthDate { get; set; } = null!;
}

public sealed record ExportExcelUserQuery() : IQuery<MemoryStream>;

public sealed class ExportExcelUserHandler(MembershipDbContext dbContext)
    : IQueryHandler<ExportExcelUserQuery, MemoryStream>
{
    public async Task<MemoryStream> Handle(
        ExportExcelUserQuery request,
        CancellationToken cancellationToken
    )
    {
        List<User> users = await dbContext.Users
            .AsNoTracking()
            .Where(x => x.Enable)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        List<ExportExcelUserDto> exportExcelUserDtos = users
            .Select(
                x =>
                    new ExportExcelUserDto
                    {
                        Name = x.Name,
                        Rut = x.Rut,
                        Email = x.Email ?? string.Empty,
                        BirthDate = x.Birthdate.ToString("dd/MM/yyyy")
                    }
            )
            .ToList();

        MemoryStream memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(
            exportExcelUserDtos,
            sheetName: "Usuarios",
            cancellationToken: cancellationToken
        );
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }
}
