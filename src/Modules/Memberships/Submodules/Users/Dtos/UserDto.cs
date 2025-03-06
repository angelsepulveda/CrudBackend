namespace Memberships.Submodules.Users.Dtos;

public sealed record UserDto(Guid Id, string Name, string Rut, string? Email, DateTime BirthDate);
