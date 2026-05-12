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

    public static DomainResult<EventType> Create(
        EventTypeScope scope,
        Guid? userId,
        Guid? calendarId,
        string name,
        string? color,
        int priority,
        Guid? id = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return DomainResult<EventType>.Failure(
                new("event_type.name_required", "Event type name is required."));
        }

        switch (scope)
        {
            case EventTypeScope.System:
                {
                    userId = null;
                    calendarId = null;
                    break;
                }

            case EventTypeScope.User:
                {
                    if (userId is null || userId == Guid.Empty)
                    {
                        return DomainResult<EventType>.Failure(new("event_type.user_required", "User id is required."));
                    }

                    calendarId = null;
                    break;
                }

            case EventTypeScope.Calendar:
                {
                    if (userId is null || userId == Guid.Empty)
                    {
                        return DomainResult<EventType>.Failure(new("event_type.user_required", "User id is required."));
                    }

                    if (calendarId is null || calendarId == Guid.Empty)
                    {
                        return DomainResult<EventType>.Failure(new("event_type.calendar_required", "Calendar id is required."));
                    }
                    break;
                }
            default:
                {
                    return DomainResult<EventType>.Failure(
                        new("event_type.invalid_scope", "Invalid event type scope."));
                }
        }

        if (priority < 0)
        {
            return DomainResult<EventType>.Failure(new("event_type.invalid_priority", "Priority must be positive."));
        }

        var eventType = new EventType(id ?? Guid.NewGuid())
        {
            Scope = scope,
            UserId = userId,
            CalendarId = calendarId,
            Name = name.Trim(),
            Color = color?.Trim(),
            Priority = priority,
        };

        return DomainResult<EventType>.Success(eventType);
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