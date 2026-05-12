using CalendarAPI.Application.Common.Results;
using MediatR;

namespace CalendarAPI.Application.CalendarEvents.Commands;
public sealed record UpdateCalendarEventCommand(
    Guid EventId,
    Guid EventTypeId,
    string Title,
    string? Description,
    DateTimeOffset StartAtUtc,
    DateTimeOffset EndAtUtc,
    IReadOnlyCollection<Guid> ParticipantIds) 
    : IRequest<Result>;