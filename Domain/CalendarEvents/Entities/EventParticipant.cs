using CalendarAPI.Domain.Common;

namespace CalendarAPI.Domain.CalendarEvents.Entities;
public enum EventParticipantStatus
{
    Invited = 1,
    Accepted = 2,
    Declined = 3
}

public sealed class EventParticipant
    : DomainEntity
{
    public Guid EventId { get; private set; }
    public Guid UserId { get; private set; }
    public EventParticipantStatus Status { get; private set; }

    private EventParticipant(Guid id)
        : base(id) { }


    public static EventParticipant Create(
        Guid eventId,
        Guid userId,
        EventParticipantStatus status,
        Guid? id = null)
    {
        return new EventParticipant(id ?? Guid.NewGuid())
        {
            EventId = eventId,
            UserId = userId,
            Status = status,
        };
    }

    public static EventParticipant Rehydrate(
        Guid id,
        Guid eventId,
        Guid userId,
        EventParticipantStatus status)
    {
        return new EventParticipant(id)
        {
            EventId = eventId,
            UserId = userId,
            Status = status,
        };
    }
}