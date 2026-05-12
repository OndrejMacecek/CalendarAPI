using CalendarAPI.Domain.CalendarEvents.Entities;
using CalendarAPI.Infrastructure.Persistence.Entities;
using CalendarAPI.Infrastructure.Users;

namespace CalendarAPI.Infrastructure.CalendarEvents;

public sealed class EventParticipantEntity 
    : DbEntity
{
    public Guid EventId { get; set; }
    public CalendarEventEntity Event { get; set; } = default!;

    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = default!;

    public EventParticipantStatus Status { get; set; }
}