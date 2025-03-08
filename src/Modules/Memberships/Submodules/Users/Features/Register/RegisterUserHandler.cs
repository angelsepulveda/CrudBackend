using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Dtos;
using Memberships.Submodules.Users.Exceptions;

namespace Memberships.Submodules.Users.Features.Register;

public sealed record RegisterUserPayload(
    string Name,
    string Rut,
    string? Email,
    DateTime BirthDate
);

public sealed record RegisterUserCommand(RegisterUserPayload Payload) : ICommand<UserDto>;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Payload.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Payload.Rut).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Payload.Email)
            .MaximumLength(256)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Payload.Email));
        ;
        RuleFor(x => x.Payload.BirthDate).NotNull();
    }
}

public sealed class RegisterUserHandler(
    MembershipDbContext dbContext,
    IGetByRutUserService getByRutUserService
) : ICommandHandler<RegisterUserCommand, UserDto>
{
    public async Task<UserDto> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        string feature = nameof(RegisterUserHandler);

        Log.Information(
            "Feature: {Feature}, inicio de resgistro de usuario request: {Request}",
            feature,
            JsonSerializer.Serialize(request)
        );

        User? userExists = await getByRutUserService.HandleAsync(request.Payload.Rut);

        if (userExists is not null)
            throw new UserRutAlreadyExistException(request.Payload.Rut);

        User user = User.Create(
            name: request.Payload.Name,
            rut: request.Payload.Rut,
            email: request.Payload.Email,
            birthdate: request.Payload.BirthDate
        );

        Log.Information(
            "Feature: {Feature}, sea creó el usuario: {User}",
            feature,
            JsonSerializer.Serialize(user)
        );

        dbContext.Users.Add(user);

        int result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result == 0)
        {
            Log.Error(
                "Feature: {Feature}, no se pudo registrar el usuario: {User}",
                feature,
                JsonSerializer.Serialize(user)
            );
            throw new BadRequestException("No se pudo registrar el usuario");
        }

        return new UserDto(
            Id: user.Id.Value,
            Rut: user.Rut,
            Name: user.Name,
            Email: user.Email,
            BirthDate: user.Birthdate
        );
    }
}
