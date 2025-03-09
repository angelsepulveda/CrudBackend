using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Dtos;
using Memberships.Submodules.Users.Features.Authenticate;
using Shared.Exceptions;

namespace Memberships.UnitTests.Submodules.Users.Features;

public class UserAuthenticateHandlerUnitTests
{
    private readonly Mock<IValidateTokenService> _validateTokenServiceMock;
    private readonly Mock<IGenerateTokenService> _generateTokenServiceMock;
    private readonly UserAuthenticateHandler _handler;

    public UserAuthenticateHandlerUnitTests()
    {
        _validateTokenServiceMock = new Mock<IValidateTokenService>();
        _generateTokenServiceMock = new Mock<IGenerateTokenService>();
        _handler = new UserAuthenticateHandler(
            _validateTokenServiceMock.Object,
            _generateTokenServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnJwtToken_WhenTokenIsValid()
    {
        // Arrange
        string token = "valid_token";
        var userProfile = new UserProfileDto("user@example.com", "User Name");
        string expectedJwtToken = "jwt_token";

        _validateTokenServiceMock
            .Setup(service => service.HandleAsync(token))
            .ReturnsAsync(userProfile);
        _generateTokenServiceMock
            .Setup(service => service.Handle(userProfile))
            .Returns(expectedJwtToken);

        var query = new UserAuthenticateQuery(token);

        // Act
        string result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(expectedJwtToken, result);
        _validateTokenServiceMock.Verify(service => service.HandleAsync(token), Times.Once);
        _generateTokenServiceMock.Verify(service => service.Handle(userProfile), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenTokenIsInvalid()
    {
        // Arrange
        string token = "invalid_token";

        _validateTokenServiceMock
            .Setup(service => service.HandleAsync(token))
            .ReturnsAsync((UserProfileDto)null);

        var query = new UserAuthenticateQuery(token);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(query, CancellationToken.None)
        );
        _validateTokenServiceMock.Verify(service => service.HandleAsync(token), Times.Once);
        _generateTokenServiceMock.Verify(
            service => service.Handle(It.IsAny<UserProfileDto>()),
            Times.Never
        );
    }
}
