using Memberships.Submodules.Users.Features.ExportExcel;
using Memberships.Submodules.Roles.Entities;

namespace Memberships.UnitTests.Submodules.Users.Features;

public class ExportExcelUserHandlerTests
{
    private readonly MembershipDbContext _dbContextMock;
    private readonly ExportExcelUserHandler _handler;

    public ExportExcelUserHandlerTests()
    {
        var options = new DbContextOptionsBuilder<MembershipDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // BD en memoria
            .Options;

        _dbContextMock = new MembershipDbContext(options);

        _handler = new ExportExcelUserHandler(_dbContextMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnMemoryStream_WhenUsersExist()
    {
        // Arrange
        var users = new List<User>
        {
            User.Create(
                name: "John Doe",
                rut: "12.345.678-9",
                email: "john@example.com",
                birthdate: new DateTime(1990, 1, 1)
            ),
            User.Create(
                name: "John Doe",
                rut: "98.765.432-1",
                email: "john@example.com",
                birthdate: new DateTime(1990, 1, 1)
            ),
        };
        await _dbContextMock.Users.AddRangeAsync(
            new List<User>
            {
                User.Create(
                    name: "John Doe",
                    rut: "12.345.678-9",
                    email: "john@example.com",
                    birthdate: new DateTime(1990, 1, 1)
                ),
                User.Create(
                    name: "John Doe",
                    rut: "98.765.432-1",
                    email: "john@example.com",
                    birthdate: new DateTime(1990, 1, 1)
                ),
            }
        );

        await _dbContextMock.SaveChangesAsync();

        var handler = new ExportExcelUserHandler(_dbContextMock);
        var query = new ExportExcelUserQuery();
        var cancellationToken = new CancellationToken();

        // Act
        MemoryStream result = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);
    }
}
