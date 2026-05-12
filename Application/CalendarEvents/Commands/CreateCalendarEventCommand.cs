using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.CalendarEvents.Commands;

public sealed record CreateCalendarEventCommand(
    Guid CalendarId,
    Guid EventTypeId,
    string Title,
    string? Description,
    DateTimeOffset StartAtUtc,
    DateTimeOffset EndAtUtc,
    IReadOnlyCollection<Guid> ParticipantIds)
    : ICommand<Guid>;