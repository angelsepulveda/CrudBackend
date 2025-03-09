using Memberships.Submodules.Users.Dtos;

namespace Memberships.Submodules.Users.Contracts.Services;

public interface IGenerateTokenService
{
    string Handle(UserProfileDto userProfileDto);
}
