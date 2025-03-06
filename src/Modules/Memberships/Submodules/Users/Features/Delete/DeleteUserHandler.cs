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
        User userDeleted = await getByIdUserService.HandleAsync(new UserId(request.Id));

        userDeleted.Delete();

        dbContext.Users.Update(userDeleted);

        int result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result == 0)
            throw new BadRequestException("No se pudo eliminar el usuario");

        return Unit.Value;
    }
}
