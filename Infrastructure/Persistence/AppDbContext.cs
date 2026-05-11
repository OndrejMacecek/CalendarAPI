using CalendarAPI.Infrastructure.Calendars.Entities;
using CalendarAPI.Infrastructure.Common;
using CalendarAPI.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.Persistence;
public sealed class AppDbContext
    : DbContext
{
    private readonly ITimeProvider _timeProvider;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ITimeProvider timeProvider
        )
        : base(options)
    {
        _timeProvider = timeProvider;
    }

    public DbSet<CalendarEntity> Calendars => Set<CalendarEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyEntityBehaviors();
        return await base.SaveChangesAsync();
    }

    private void ApplyEntityBehaviors()
    {
        var now = _timeProvider.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity is DbEntity audit)
                {
                    audit.CreatedAtUtc = now;
                    audit.UpdatedAtUtc = now;
                }
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Entity is DbEntity audit)
                {
                    audit.UpdatedAtUtc = now;
                }
            }
        }
    }
}