using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace Memberships.Submodules.Users.Services;

internal sealed class GenerateJwtTokenService : IGenerateTokenService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _secretKey;

    public GenerateJwtTokenService(string issuer, string audience, string secretKey)
    {
        _issuer = issuer;
        _audience = audience;
        _secretKey = secretKey;
    }

    public string Handle(UserProfileDto userProfileDto)
    {
        Claim[] claims =
        {
            new Claim(JwtRegisteredClaimNames.Sub, userProfileDto.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, userProfileDto.Name),
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: credentials
        );

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        string tokenString = tokenHandler.WriteToken(jwtToken);

        return tokenString;
    }
}
