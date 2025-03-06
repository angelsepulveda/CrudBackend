using MediatR;
using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Exceptions;
using Memberships.Submodules.Users.Features.Update;
using Memberships.Submodules.Users.ValueObjects;
using Shared.Exceptions;

namespace Memberships.UnitTests.Submodules.Users.Features
{
    public class UpdateUserHandlerUnitTest
    {
        private readonly Mock<IGetByIdUserService> _mockGetByIdUserService;
        private readonly Mock<IGetByRutUserService> _mockGetByRutUserService;
        private readonly MembershipDbContext _dbContext;
        private readonly UpdateUserHandler _handler;
        private readonly UpdateUserCommandValidator _validator;

        public UpdateUserHandlerUnitTest()
        {
            _mockGetByIdUserService = new Mock<IGetByIdUserService>();
            _mockGetByRutUserService = new Mock<IGetByRutUserService>();

            var options = new DbContextOptionsBuilder<MembershipDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new MembershipDbContext(options);
            _handler = new UpdateUserHandler(
                _dbContext,
                _mockGetByIdUserService.Object,
                _mockGetByRutUserService.Object
            );
            _validator = new UpdateUserCommandValidator();
        }

        [Fact]
        public async Task Handle_ShouldUpdateUserSuccessfully()
        {
            // Arrange
            User existingUser = User.Create(
                "John Doe",
                "19.108.386-5",
                "john.updated@example.com",
                DateTime.UtcNow.AddYears(-30)
            );
            _dbContext.Users.Add(existingUser);
            await _dbContext.SaveChangesAsync();

            _mockGetByIdUserService
                .Setup(s => s.HandleAsync(It.IsAny<UserId>()))
                .ReturnsAsync(existingUser);

            _mockGetByRutUserService
                .Setup(s => s.HandleAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            UpdateUserCommand command =
                new(
                    new UpdateUserPayload(
                        existingUser.Id.Value,
                        "John Updated",
                        "19.108.386-5",
                        "john.updated@example.com",
                        DateTime.UtcNow.AddYears(-29)
                    )
                );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            User? updatedUser = await _dbContext.Users.FindAsync(existingUser.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal("John Updated", updatedUser.Name);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            _mockGetByIdUserService
                .Setup(s => s.HandleAsync(It.IsAny<UserId>()))
                .ThrowsAsync(new NotFoundException("User not found"));

            UpdateUserCommand command =
                new(
                    new UpdateUserPayload(
                        Guid.NewGuid(),
                        "John Updated",
                        "19.108.386-5",
                        "john.updated@example.com",
                        DateTime.UtcNow.AddYears(-29)
                    )
                );

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRutAlreadyExists()
        {
            // Arrange
            User existingUser = User.Create(
                "John Doe",
                "19.108.386-5",
                "john@example.com",
                DateTime.UtcNow.AddYears(-30)
            );
            User anotherUser = User.Create(
                "Jane Doe",
                "11.222.333-4",
                "jane@example.com",
                DateTime.UtcNow.AddYears(-25)
            );

            _dbContext.Users.AddRange(existingUser, anotherUser);
            await _dbContext.SaveChangesAsync();

            _mockGetByIdUserService
                .Setup(s => s.HandleAsync(It.IsAny<UserId>()))
                .ReturnsAsync(existingUser);

            _mockGetByRutUserService
                .Setup(s => s.HandleAsync("11.222.333-4"))
                .ReturnsAsync(anotherUser);

            UpdateUserCommand command =
                new(
                    new UpdateUserPayload(
                        existingUser.Id.Value,
                        "John Updated",
                        "11.222.333-4",
                        "john.updated@example.com",
                        DateTime.UtcNow.AddYears(-29)
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
            UpdateUserCommand command =
                new(
                    new UpdateUserPayload(
                        Id: Guid.NewGuid(),
                        Rut: "",
                        Name: "",
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
}
