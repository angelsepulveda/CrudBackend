using Memberships.Submodules.Users.ValueObjects;

namespace Memberships.Submodules.Roles.Entities;

public class User : Entity<UserId>
{
    public string Name { get; private set; }
    public string Rut { get; private set; }
    public string? Email { get; private set; }
    public DateTime Birthdate { get; private set; }
    public bool Enable { get; private set; }

    private User(UserId id, string name, string rut, string? email, DateTime birthdate, bool enable)
    {
        Id = id;
        Name = name;
        Rut = rut;
        Email = email;
        Birthdate = birthdate;
        Enable = enable;
    }

    public static User Create(string name, string rut, string? email, DateTime birthdate)
    {
        const bool enable = true;

        if (string.IsNullOrEmpty(email))
            email = null;

        return new User(new UserId(Guid.NewGuid()), name, rut, email, birthdate, enable);
    }

    public void Update(string name, string rut, string? email, DateTime birthdate)
    {
        if (string.IsNullOrEmpty(email))
            Email = null;

        Name = name;
        Rut = rut;
        Birthdate = birthdate;
    }

    public void Delete()
    {
        Enable = false;
    }
}
