using CalendarAPI.Infrastructure.Persistence.Entities;

namespace CalendarAPI.Infrastructure.Calendars.Entities;

public sealed class CalendarEntity 
    : DbEntity
{
    public string Name { get; set; } = default!;
}
