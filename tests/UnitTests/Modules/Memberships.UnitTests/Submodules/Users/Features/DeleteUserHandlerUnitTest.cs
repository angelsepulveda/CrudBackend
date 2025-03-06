using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Features.Delete;
using Memberships.Submodules.Users.ValueObjects;

namespace Memberships.UnitTests.Submodules.Users.Features;

public class DeleteUserHandlerTests
{
    private readonly Mock<IGetByIdUserService> _mockGetByIdUserService;
    private readonly MembershipDbContext _dbContext;
    private readonly DeleteUserHandler _handler;

    public DeleteUserHandlerTests()
    {
        // Usamos un DbContext en memoria para pruebas
        var options = new DbContextOptionsBuilder<MembershipDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new MembershipDbContext(options);

        _mockGetByIdUserService = new Mock<IGetByIdUserService>();
        _handler = new DeleteUserHandler(_dbContext, _mockGetByIdUserService.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenRoleExists()
    {
        // Arrange
        User user = User.Create(
            name: "Angel sepulveda sepulveda",
            rut: "19.108.386-5",
            email: "dewebsic@gmail.com",
            birthdate: DateTime.Now
        );
        Guid userId = user.Id.Value;

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        _mockGetByIdUserService
            .Setup(service => service.HandleAsync(It.IsAny<UserId>()))
            .ReturnsAsync(user);

        DeleteUserCommand command = new(userId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        User? deletedUser = await _dbContext.Users
            .Where(x => x.Id == user.Id && x.Enable)
            .FirstOrDefaultAsync();

        Assert.Null(deletedUser);
    }
}
