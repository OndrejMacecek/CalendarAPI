using CalendarAPI.Infrastructure.CalendarEvents;
using CalendarAPI.Infrastructure.Calendars.Entities;
using CalendarAPI.Infrastructure.Common;
using CalendarAPI.Infrastructure.EventTypes;
using CalendarAPI.Infrastructure.Persistence.Entities;
using CalendarAPI.Infrastructure.Users;
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

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<CalendarEntity> Calendars => Set<CalendarEntity>();
    public DbSet<EventTypeEntity> EventTypes => Set<EventTypeEntity>();
    public DbSet<CalendarEventEntity> CalendarEvents => Set<CalendarEventEntity>();
    public DbSet<EventParticipantEntity> EventParticipants => Set<EventParticipantEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyEntityBehaviors();

        Console.WriteLine(ChangeTracker.DebugView.LongView);

        return await base.SaveChangesAsync(cancellationToken);
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