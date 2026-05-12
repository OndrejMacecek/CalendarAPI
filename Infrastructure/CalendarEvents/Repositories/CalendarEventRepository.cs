using CalendarAPI.Domain.CalendarEvents.Aggregates;
using CalendarAPI.Domain.CalendarEvents.Entities;
using CalendarAPI.Domain.CalendarEvents.Repositories;
using CalendarAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.CalendarEvents.Repositories;
public sealed class CalendarEventRepository
    : BaseRepository<CalendarEvent, CalendarEventEntity>, ICalendarEventRepository
{
    public CalendarEventRepository(AppDbContext context)
        : base(context) { }

    public override async Task<CalendarEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await DbSet
            .AsNoTracking()
            .Include(x => x.Participants)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? null : ToDomain(entity);
    }

    public override async Task<IReadOnlyCollection<CalendarEvent>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await DbSet.AsNoTracking()
            .Include(x => x.Participants)
            .ToListAsync(cancellationToken);
        return entities.Select(ToDomain).ToList();
    }

    public override async Task UpdateAsync(CalendarEvent domain, CancellationToken cancellationToken)
    {
        var entity = await DbSet
            .Include(x => x.Participants)
            .FirstOrDefaultAsync(x => x.Id == domain.Id, cancellationToken);

        if (entity is null)
            return;

        MapToExistingEntity(domain, entity);
    }

    public async Task<IReadOnlyCollection<CalendarEvent>> GetByCalendarIdInRangeAsync(
        Guid calendarId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken cancellationToken)
    {
        var from = fromUtc.UtcDateTime;
        var to = toUtc.UtcDateTime;

        var entities = await DbSet
            .AsNoTracking()
            .Include(x => x.Participants)
            .Where(x =>
                x.CalendarId == calendarId &&
                x.StartAtUtc < to &&
                x.EndAtUtc > from)
            .ToListAsync(cancellationToken);

        return entities.Select(ToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<CalendarEvent>> GetVisibleForUserInRangeAsync(
        Guid userId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken cancellationToken)
    {
        var from = fromUtc.UtcDateTime;
        var to = toUtc.UtcDateTime;

        var entities = await DbSet
            .AsNoTracking()
            .Include(x => x.Participants)
            .Where(x =>
                x.StartAtUtc < to &&
                x.EndAtUtc > from &&
                (
                    x.OwnerUserId == userId ||
                    x.Participants.Any(p =>
                        p.UserId == userId &&
                        p.Status != EventParticipantStatus.Declined)
                ))
            .ToListAsync(cancellationToken);

        return entities.Select(ToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<CalendarEvent>> GetInvitationsForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entities = await DbSet.AsNoTracking()
            .Include(x => x.Participants)
            .Where(x => x.Participants.Any(p => p.UserId == userId))
            .ToListAsync(cancellationToken);

        return entities.Select(ToDomain).ToList();
    }

    public async Task<bool> HasConflictAsync(
        Guid userId, DateTimeOffset startAtUtc, DateTimeOffset endAtUtc, Guid? ignoredEventId, CancellationToken cancellationToken)
    {
        var start = startAtUtc.UtcDateTime;
        var end = endAtUtc.UtcDateTime;

        return await DbSet.AsNoTracking()
            .Include(x => x.Participants)
            .AnyAsync(x =>
                (ignoredEventId == null || x.Id != ignoredEventId) &&
                x.StartAtUtc < end &&
                x.EndAtUtc > start &&

                (
                    x.OwnerUserId == userId ||

                    x.Participants.Any(p =>
                        p.UserId == userId &&
                        p.Status != EventParticipantStatus.Declined)
                ),
                cancellationToken);
    }

    protected override void MapToExistingEntity(CalendarEvent domain, CalendarEventEntity entity)
    {
        entity.EventTypeId = domain.EventTypeId;
        entity.Title = domain.Title;
        entity.Description = domain.Description;
        entity.StartAtUtc = domain.TimeRange.StartAtUtc.UtcDateTime;
        entity.EndAtUtc = domain.TimeRange.EndAtUtc.UtcDateTime;
        entity.UpdatedAtUtc = DateTime.UtcNow;

        var desiredByUserId = domain.Participants
            .GroupBy(x => x.UserId)
            .ToDictionary(x => x.Key, x => x.First());

        var participantsToRemove = entity.Participants
            .Where(x => !desiredByUserId.ContainsKey(x.UserId))
            .ToList();

        foreach (var participantToRemove in participantsToRemove)
        {
            entity.Participants.Remove(participantToRemove);
        }

        foreach (var desiredParticipant in desiredByUserId.Values)
        {
            var existingParticipant = entity.Participants
                .FirstOrDefault(x => x.UserId == desiredParticipant.UserId);

            if (existingParticipant is not null)
            {
                existingParticipant.Status = desiredParticipant.Status;
                continue;
            }

            entity.Participants.Add(new EventParticipantEntity
            {
                Id = Guid.NewGuid(),
                EventId = entity.Id,
                UserId = desiredParticipant.UserId,
                Status = desiredParticipant.Status,
            });
        }
    }

    protected override CalendarEvent ToDomain(CalendarEventEntity entity)
    {
        var participants = entity.Participants
            .Select(x => EventParticipant.Rehydrate(
                x.Id,
                x.EventId,
                x.UserId,
                x.Status))
            .ToList();

        return CalendarEvent.Rehydrate(
            entity.Id,
            entity.CalendarId,
            entity.OwnerUserId,
            entity.EventTypeId,
            entity.Title,
            entity.Description,
            DateTime.SpecifyKind(entity.StartAtUtc, DateTimeKind.Utc),
            DateTime.SpecifyKind(entity.EndAtUtc, DateTimeKind.Utc),
            participants);
    }

    protected override CalendarEventEntity ToEntity(CalendarEvent domain)
    {
        return new CalendarEventEntity
        {
            Id = domain.Id,
            CalendarId = domain.CalendarId,
            OwnerUserId = domain.OwnerUserId,
            EventTypeId = domain.EventTypeId,
            Title = domain.Title,
            Description = domain.Description,
            StartAtUtc = domain.TimeRange.StartAtUtc.UtcDateTime,
            EndAtUtc = domain.TimeRange.EndAtUtc.UtcDateTime,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            Participants = domain.Participants
                .Select(x => new EventParticipantEntity
                {
                    Id = Guid.NewGuid(),
                    EventId = domain.Id,
                    UserId = x.UserId,
                    Status = x.Status,
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow
                })
                .ToList()
        };
    }
}
