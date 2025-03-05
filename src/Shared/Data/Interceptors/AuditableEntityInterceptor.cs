using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Constants;

namespace Shared.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateEntities(DbContext? context)
    {
        if (context == null)
            return;

        foreach (EntityEntry<IEntity> entry in context.ChangeTracker.Entries<IEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = GlobalConstants.UserNameAudit;
                entry.Entity.CreatedAt = DateTime.Now;
            }

            if (
                entry.State != EntityState.Added
                && entry.State != EntityState.Modified
                && !entry.HasChangedOwnedEntities()
            )
            {
                continue;
            }
            entry.Entity.LastModifiedBy = GlobalConstants.UserNameAudit;
            entry.Entity.LastModified = DateTime.Now;
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(
            r =>
                r.TargetEntry != null
                && r.TargetEntry.Metadata.IsOwned()
                && (
                    r.TargetEntry.State == EntityState.Added
                    || r.TargetEntry.State == EntityState.Modified
                )
        );
}
