using CalendarAPI.Domain.CalendarEvents.Entities;
using CalendarAPI.Domain.Calendars.ValueObjects;
using CalendarAPI.Domain.Common;

namespace CalendarAPI.Domain.CalendarEvents.Aggregates;
public sealed class CalendarEvent
    : DomainAggregate
{
    private readonly List<EventParticipant> _participants = new();

    public Guid CalendarId { get; private set; }
    public Guid OwnerUserId { get; private set; }
    public Guid EventTypeId { get; private set; }

    public string Title { get; private set; } = null!;
    public string? Description { get; private set; } = null!;
    public EventTimeRange TimeRange { get; private set; } = null!;

    public IReadOnlyCollection<EventParticipant> Participants => _participants;

    private CalendarEvent(Guid id)
        : base(id) { }

    public static DomainResult<CalendarEvent> Create(
        Guid calendarId,
        Guid ownerUserId,
        Guid eventTypeId,
        string title,
        string? description,
        DateTimeOffset startAtUtc,
        DateTimeOffset endAtUtc,
        Guid? id = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return DomainResult<CalendarEvent>.Failure(new("event.title_required", "Event title is required."));
        }

        if (title.Length > 150)
        {
            return DomainResult<CalendarEvent>.Failure(new("event.title_too_long", "Event title must be 150 characters or less."));
        }

        var timeRangeResult = EventTimeRange.Create(startAtUtc, endAtUtc);

        if (timeRangeResult.IsFailure)
        {
            return DomainResult<CalendarEvent>.Failure(timeRangeResult.Error!);
        }

        var calendarEvent = new CalendarEvent(id ?? Guid.NewGuid())
        {
            CalendarId = calendarId,
            OwnerUserId = ownerUserId,
            EventTypeId = eventTypeId,
            Title = title,
            Description = description,
            TimeRange = timeRangeResult.Value!,
        };

        return DomainResult<CalendarEvent>.Success(calendarEvent);
    }

    public static CalendarEvent Rehydrate(
        Guid id,
        Guid calendarId,
        Guid ownerUserId,
        Guid eventTypeId,
        string title,
        string? description,
        DateTimeOffset startAtUtc,
        DateTimeOffset endAtUtc,
        IEnumerable<EventParticipant> participants)
    {
        var timeRange = EventTimeRange.Create(startAtUtc, endAtUtc).Value!;

        var calendarEvent = new CalendarEvent(id)
        {
            CalendarId = calendarId,
            OwnerUserId = ownerUserId,
            EventTypeId = eventTypeId,
            Title = title,
            Description = description,
            TimeRange = timeRange,
        };

        calendarEvent._participants.AddRange(participants);

        return calendarEvent;
    }
}