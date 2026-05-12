using CalendarAPI.Domain.EventTypes.Aggregates;
using CalendarAPI.Infrastructure.CalendarEvents;
using CalendarAPI.Infrastructure.Calendars.Entities;
using CalendarAPI.Infrastructure.Persistence.Entities;
using CalendarAPI.Infrastructure.Users;

namespace CalendarAPI.Infrastructure.EventTypes;

public sealed class EventTypeEntity 
    : DbEntity
{
    public EventTypeScope Scope { get; set; }

    public Guid? UserId { get; set; }
    public UserEntity? User { get; set; }

    public Guid? CalendarId { get; set; }
    public CalendarEntity? Calendar { get; set; }

    public string Name { get; set; } = default!;
    public string? Color { get; set; }
    public int Priority { get; set; }

    public ICollection<CalendarEventEntity> Events { get; set; } = [];
}