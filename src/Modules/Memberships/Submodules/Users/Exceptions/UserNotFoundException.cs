namespace Memberships.Submodules.Users.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException()
        : base($"Usuario no encontrado.") { }
}
