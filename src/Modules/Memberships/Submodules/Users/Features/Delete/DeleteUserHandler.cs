using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.Contracts.Services;
using Memberships.Submodules.Users.ValueObjects;

namespace Memberships.Submodules.Users.Features.Delete;

public sealed record DeleteUserCommand(Guid Id) : ICommand;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}

public sealed class DeleteUserHandler(
    MembershipDbContext dbContext,
    IGetByIdUserService getByIdUserService
) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        string feature = nameof(DeleteUserHandler);

        Log.Information(
            "Inicio proceso de eliminación usuario id: {UserId}, feature: {Feature}",
            request.Id,
            feature
        );

        User userDeleted = await getByIdUserService.HandleAsync(new UserId(request.Id));

        Log.Information(
            "se obtuvo el usuario id: {UserId}, Estado: {Usuario} feature: {Feature}",
            request.Id,
            userDeleted.Enable,
            feature
        );

        userDeleted.Delete();

        dbContext.Users.Update(userDeleted);

        int result;

        result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result == 0)
        {
            Log.Error(
                "No se pudo eliminar el usuario con id: {UserId}, feature: {Feature}",
                userDeleted.Id.Value,
                feature
            );
            throw new BadRequestException("No se pudo eliminar el usuario");
        }

        Log.Information(
            "Se eliminó exitosamente el usuario con id: {UserId}, estado: {UserState}, feature: {Feature}",
            userDeleted.Id.Value,
            userDeleted.Enable,
            feature
        );
        return Unit.Value;
    }
}
