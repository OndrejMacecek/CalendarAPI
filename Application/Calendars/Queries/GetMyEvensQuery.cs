using CalendarApi.Contract;
using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.Calendars.Queries;

public sealed record GetMyEventsQuery(
    DateTimeOffset FromUtc, DateTimeOffset ToUtc)
    : IQuery<IReadOnlyCollection<CalendarEventDto>>;