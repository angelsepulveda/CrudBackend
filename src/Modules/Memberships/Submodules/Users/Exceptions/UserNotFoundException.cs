using Memberships.Submodules.Users.ValueObjects;

namespace Memberships.Submodules.Users.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(UserId id)
        : base($"Usuario no encontrado.")
    {
        Log.Error(
            "ExceptionClass: {ExceptionClass}, error: {Error}",
            nameof(UserRutAlreadyEnabledException),
            JsonSerializer.Serialize($"Usuario no encontrado con el id: {id.Value}.")
        );
    }
}
