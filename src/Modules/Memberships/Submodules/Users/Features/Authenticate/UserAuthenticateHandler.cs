using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Dtos;

namespace Memberships.Submodules.Users.Features.Authenticate;

public sealed record TokenRequest(string Token);

public sealed record UserAuthenticateQuery(string Token) : IQuery<string>;

public sealed class UserAuthenticateHandler(
    IValidateTokenService validateToken,
    IGenerateTokenService generateTokenService
) : IQueryHandler<UserAuthenticateQuery, string>
{
    public async Task<string> Handle(
        UserAuthenticateQuery request,
        CancellationToken cancellationToken
    )
    {
        string token = request.Token;

        UserProfileDto? user = await validateToken.HandleAsync(token);

        if (user is null)
            throw new BadRequestException("Ocurrio un error al autenticar");

        string jwtToken = generateTokenService.Handle(user);

        return jwtToken;
    }
}
