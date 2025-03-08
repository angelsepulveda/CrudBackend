using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.Exceptions;
using Memberships.Submodules.Users.ValueObjects;

namespace Memberships.Submodules.Users.Features.Update;

public sealed record UpdateUserPayload(
    Guid Id,
    string Name,
    string Rut,
    string? Email,
    DateTime BirthDate
);

public sealed record UpdateUserCommand(UpdateUserPayload Payload) : ICommand;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
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

public sealed class UpdateUserHandler(
    MembershipDbContext dbContext,
    IGetByIdUserService getByIdUserService,
    IGetByRutUserService getByRutUserService
) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        string feature = nameof(UpdateUserHandler);

        Log.Information(
            "Feature: {Feature}, inició de actualización de usuario request: {Request}",
            feature,
            JsonSerializer.Serialize(request)
        );

        User userUpdated = await getByIdUserService.HandleAsync(new UserId(request.Payload.Id));

        Log.Information(
            "Feature: {Feature}, usuario a actualizar request: {Request}",
            feature,
            JsonSerializer.Serialize(userUpdated)
        );

        User? userExists = await getByRutUserService.HandleAsync(request.Payload.Rut);

        if (userExists is not null && userExists.Id.Value != request.Payload.Id)
            throw new UserRutAlreadyExistException(request.Payload.Rut);

        userUpdated.Update(
            name: request.Payload.Name,
            rut: request.Payload.Rut,
            birthdate: request.Payload.BirthDate,
            email: request.Payload.Email
        );

        dbContext.Users.Update(userUpdated);

        int result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result == 0)
        {
            Log.Error(
                "Feature: {Feature}, no se pudo actualizar el usuario: {User}",
                feature,
                JsonSerializer.Serialize(userUpdated)
            );
            throw new BadRequestException("No se pudo actualizar el usuario");
        }

        return Unit.Value;
    }
}
