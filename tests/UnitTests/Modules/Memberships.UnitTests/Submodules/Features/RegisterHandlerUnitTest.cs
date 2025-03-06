using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Dtos;
using Memberships.Submodules.Users.Exceptions;
using Memberships.Submodules.Users.Features.Register;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shared.Exceptions;
using FluentValidation.TestHelper;

namespace Memberships.UnitTests.Submodules.Users.Features;

public class RegisterUserHandlerUnitTest
{
    private readonly Mock<IGetByRutUserService> _mockGetByRutUserService;
    private readonly MembershipDbContext _dbContext;
    private readonly RegisterUserHandler _handler;
    private readonly RegisterUserCommandValidator _validator;

    public RegisterUserHandlerUnitTest()
    {
        _mockGetByRutUserService = new Mock<IGetByRutUserService>();

        var options = new DbContextOptionsBuilder<MembershipDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // BD en memoria
            .Options;

        _dbContext = new MembershipDbContext(options);
        _handler = new RegisterUserHandler(_dbContext, _mockGetByRutUserService.Object);
        _validator = new RegisterUserCommandValidator();
    }

    [Fact]
    public async Task Handle_ShouldRegisterUserSuccessfully()
    {
        // Arrange
        _mockGetByRutUserService
            .Setup(service => service.HandleAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        RegisterUserCommand command =
            new(
                new RegisterUserPayload(
                    Name: "Angel Sepulveda Sepulveda",
                    Rut: "19.108.386-5",
                    Email: "dewebsic@gmail.com",
                    BirthDate: DateTime.UtcNow.AddYears(-25)
                )
            );

        // Act
        UserDto result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Angel Sepulveda Sepulveda", result.Name);

        User? userInDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.Rut == "19.108.386-5");
        Assert.NotNull(userInDb);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRutAlreadyExists()
    {
        // Arrange
        User existingUser = User.Create(
            "Juan Perez",
            "19.108.386-5",
            "juan@example.com",
            DateTime.UtcNow.AddYears(-30)
        );

        _dbContext.Users.Add(existingUser);
        await _dbContext.SaveChangesAsync();

        _mockGetByRutUserService
            .Setup(service => service.HandleAsync("19.108.386-5"))
            .ReturnsAsync(existingUser);

        RegisterUserCommand command =
            new(
                new RegisterUserPayload(
                    Name: "Otro Usuario",
                    Rut: "19.108.386-5",
                    Email: "otro@example.com",
                    BirthDate: DateTime.UtcNow.AddYears(-20)
                )
            );

        // Act & Assert
        await Assert.ThrowsAsync<UserRutAlreadyExistException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public void Validator_ShouldFail_WhenInvalidData()
    {
        // Arrange
        RegisterUserCommand command =
            new(
                new RegisterUserPayload(
                    Name: "",
                    Rut: "",
                    Email: "invalid-email",
                    BirthDate: DateTime.Now
                )
            );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Payload.Name);
        result.ShouldHaveValidationErrorFor(x => x.Payload.Rut);
        result.ShouldHaveValidationErrorFor(x => x.Payload.Email);
    }
}
