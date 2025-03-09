using Memberships.Submodules.Users.Dtos;

namespace Memberships.Submodules.Users.Contracts.Services;

public interface IValidateTokenService
{
    Task<UserProfileDto> HandleAsync(string token);
}
