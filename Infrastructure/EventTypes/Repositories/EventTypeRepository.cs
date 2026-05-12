using CalendarAPI.Domain.EventTypes.Aggregates;
using CalendarAPI.Domain.EventTypes.Repositories;
using CalendarAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.EventTypes.Repositories;
public sealed class EventTypeRepository
    : BaseRepository<EventType, EventTypeEntity>, IEventTypeRepository
{
    public EventTypeRepository(AppDbContext context)
        : base(context) { }

    public async Task<IReadOnlyCollection<EventType>> GetAvailableAsync(Guid userId, Guid calendarId, CancellationToken cancellationToken)
    {
        var entities = await DbSet.AsNoTracking()
           .Where(x =>
               x.Scope == EventTypeScope.System ||
               x.Scope == EventTypeScope.User && x.UserId == userId ||
               x.Scope == EventTypeScope.Calendar &&
               x.UserId == userId &&
               x.CalendarId == calendarId)
           .ToListAsync(cancellationToken);

        return entities.Select(ToDomain).ToList();
    }

    protected override void MapToExistingEntity(EventType domain, EventTypeEntity entity)
    {
        entity.Scope = domain.Scope;
        entity.UserId = domain.UserId;
        entity.CalendarId = domain.CalendarId;
        entity.Name = domain.Name;
        entity.Color = domain.Color;
        entity.Priority = domain.Priority;
    }

    protected override EventType ToDomain(EventTypeEntity entity)
    {
        return EventType.Rehydrate(
            entity.Id,
            entity.Scope,
            entity.UserId,
            entity.CalendarId,
            entity.Name,
            entity.Color,
            entity.Priority);
    }

    protected override EventTypeEntity ToEntity(EventType domain)
    {
        return new EventTypeEntity
        {
            Id = domain.Id,
            Scope = domain.Scope,
            UserId = domain.UserId,
            CalendarId = domain.CalendarId,
            Name = domain.Name,
            Color = domain.Color,
            Priority = domain.Priority
        };
    }
}
