namespace Memberships.Submodules.Users.Exceptions;

public class UserRutAlreadyExistException : BadRequestException
{
    public UserRutAlreadyExistException(string rut)
        : base($"El usuario con el RUT '{rut}' ya existe.") { }
}
