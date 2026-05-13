using CalendarApi.Contract;
using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.CalendarEvents.Queries;
public sealed record GetCalendarEventsQuery(
    Guid CalendarId, DateTimeOffset FromUtc, DateTimeOffset ToUtc)
    : IQuery<IReadOnlyCollection<CalendarEventDto>>;