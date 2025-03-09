using System.Text.Json.Nodes;
using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Dtos;

internal sealed class ValidateGoogleTokenService : IValidateTokenService
{
    public async Task<UserProfileDto> HandleAsync(string token)
    {
        using (var httpClient = new HttpClient())
        {
            var googleResponse = await httpClient.GetStringAsync(
                $"https://oauth2.googleapis.com/tokeninfo?id_token={token}"
            );

            JsonObject? jsonObject = JsonSerializer.Deserialize<JsonObject>(googleResponse);

            var userId = jsonObject?["email"]?.ToString() ?? string.Empty;
            var userName = jsonObject?["name"]?.ToString() ?? string.Empty;

            return new UserProfileDto(userId, userName);
        }
    }
}
