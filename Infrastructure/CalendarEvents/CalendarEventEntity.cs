using CalendarAPI.Infrastructure.Calendars.Entities;
using CalendarAPI.Infrastructure.EventTypes;
using CalendarAPI.Infrastructure.Persistence.Entities;
using CalendarAPI.Infrastructure.Users;

namespace CalendarAPI.Infrastructure.CalendarEvents;

public sealed class CalendarEventEntity : DbEntity
{
    public Guid CalendarId { get; set; }
    public CalendarEntity Calendar { get; set; } = default!;

    public Guid OwnerUserId { get; set; }
    public UserEntity OwnerUser { get; set; } = default!;

    public Guid EventTypeId { get; set; }
    public EventTypeEntity EventType { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public DateTime StartAtUtc { get; set; }
    public DateTime EndAtUtc { get; set; }

    public ICollection<EventParticipantEntity> Participants { get; set; } = [];
}
