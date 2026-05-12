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

    public DomainResult InviteParticipant(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return DomainResult.Failure(
                new DomainError("event.invalid_participant", "Participant id is invalid."));
        }

        if (_participants.Any(x => x.UserId == userId))
        {
            return DomainResult.Failure(
                new DomainError("event.participant_already_exists", "Participant already exists."));
        }

        _participants.Add(EventParticipant.Create(Id, userId));

        return DomainResult.Success();
    }

    public DomainResult AcceptInvitation(Guid userId)
    {
        var participant = _participants.FirstOrDefault(x => x.UserId == userId);

        if (participant is null)
        {
            return DomainResult.Failure(
                new DomainError("invitation.not_found", "Invitation was not found."));
        }

        participant.Accept();

        return DomainResult.Success();
    }

    public DomainResult DeclineInvitation(Guid userId)
    {
        var participant = _participants.FirstOrDefault(x => x.UserId == userId);

        if (participant is null)
        {
            return DomainResult.Failure(
                new DomainError("invitation.not_found", "Invitation was not found."));
        }

        participant.Decline();

        return DomainResult.Success();
    }

    public DomainResult Update(
        Guid eventTypeId,
        string title,
        string? description,
        DateTimeOffset startAtUtc,
        DateTimeOffset endAtUtc,
        IReadOnlyCollection<Guid> participantIds)
    {

        var timeRangeResult = EventTimeRange.Create(startAtUtc, endAtUtc);

        if (timeRangeResult.IsFailure)
        {
            return DomainResult<CalendarEvent>.Failure(timeRangeResult.Error!);
        }

        TimeRange = timeRangeResult.Value!;
        EventTypeId = eventTypeId;
        Title = title.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

        var desiredIds = participantIds.Distinct().ToHashSet();

        _participants.RemoveAll(p => !desiredIds.Contains(p.UserId));

        var existingIds = Participants.Select(p => p.UserId).ToHashSet();

        foreach (var participantId in desiredIds.Except(existingIds))
        {
            _participants.Add(
                EventParticipant.Create(Id, participantId));
        }

        return DomainResult.Success();
    }
}