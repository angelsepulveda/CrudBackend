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
        string feature = nameof(UserAuthenticateHandler);

        string token = request.Token;

        Log.Information(
            "Feature: {Feature}, inicio de sesion de usuario request: {Request} y token:{Token}",
            feature,
            JsonSerializer.Serialize(request),
            token
        );

        UserProfileDto? user = await validateToken.HandleAsync(token);

        if (user is null)
            throw new BadRequestException("Ocurrio un error al autenticar");

        string jwtToken = generateTokenService.Handle(user);

        Log.Information("Feature: {Feature},  se obtiene el token:{Token}", feature, token);

        return jwtToken;
    }
}
