using CalendarAPI.Domain.Common;

namespace CalendarAPI.Domain.EventTypes.Aggregates;

public enum EventTypeScope
{
    System = 1, //for all
    User = 2, // for user
    Calendar = 3 // for calendar
}

public sealed class EventType
    : DomainAggregate
{
    public EventTypeScope Scope { get; private set; }
    public Guid? UserId { get; private set; }
    public Guid? CalendarId { get; private set; }

    public string Name { get; private set; } = null!;
    public string? Color { get; private set; } = null!;
    public int Priority { get; private set; }

    private EventType(Guid id)
        : base(id) { }

    public static EventType Create(
        EventTypeScope scope,
        Guid? userId,
        Guid? calendarId,
        string name,
        string? color,
        int priority,
        Guid? id)
    {
        return new EventType(id ?? Guid.NewGuid())
        {
            CalendarId = calendarId,
            Name = name,
            Color = color,
            Priority = priority,
            Scope = scope,
            UserId = userId,
        };
    }

    public static EventType Rehydrate(
        Guid id,
        EventTypeScope scope,
        Guid? userId,
        Guid? calendarId,
        string name,
        string? color,
        int priority)
    {
        return new EventType(id)
        {
            CalendarId = calendarId,
            UserId = userId,
            Name = name,
            Color = color,
            Priority = priority,
            Scope = scope
        };
    }
}