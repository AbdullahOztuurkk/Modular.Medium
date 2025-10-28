using BuildingBlocks.Domain.Common;
using BuildingBlocks.Domain.Enums;
using BuildingBlocks.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Database.EntityFrameworkCore.Interceptors;

public class AuditEntityInterceptor(ICurrentUser currentUser) : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser = currentUser;

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        if (context == null)
            return base.SavingChanges(eventData, result);

        SetAuditFields(context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        SetAuditFields(context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetAuditFields(DbContext context)
    {
        var entries = context.ChangeTracker.Entries<AuditEntity>();

        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow.AddHours(3);

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreateDate = now;
                    entry.Entity.CreateUserId = _currentUser.Id;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateUserId = _currentUser.Id;
                    entry.Entity.UpdateDate = now;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeleteDate = now;
                    entry.Entity.DeleteUserId = _currentUser.Id;
                    entry.Entity.Status = StatusType.Passive;
                    break;
            }
        }
    }
}