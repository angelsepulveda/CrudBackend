namespace Memberships.Submodules.Users.Exceptions;

public class UserRutAlreadyEnabledException : BadRequestException
{
    public UserRutAlreadyEnabledException(string rut)
        : base($"El usuario con el RUT {rut} ya existe y está habilitado.")
    {
        Log.Error(
            "ExceptionClass: {ExceptionClass}, error: {Error}",
            nameof(UserRutAlreadyEnabledException),
            JsonSerializer.Serialize($"El usuario con el RUT {rut} ya existe y está habilitado.")
        );
    }
}
