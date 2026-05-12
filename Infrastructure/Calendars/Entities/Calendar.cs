using CalendarAPI.Infrastructure.CalendarEvents;
using CalendarAPI.Infrastructure.EventTypes;
using CalendarAPI.Infrastructure.Persistence.Entities;
using CalendarAPI.Infrastructure.Users;

namespace CalendarAPI.Infrastructure.Calendars.Entities;

public sealed class CalendarEntity 
    : DbEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = default!;

    public string Name { get; set; } = default!;
    public string TimeZoneId { get; set; } = "UTC";

    public ICollection<CalendarEventEntity> Events { get; set; } = [];
    public ICollection<EventTypeEntity> EventTypes { get; set; } = [];
}
