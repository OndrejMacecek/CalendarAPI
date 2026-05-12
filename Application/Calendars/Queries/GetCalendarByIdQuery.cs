using CalendarApi.Contract;
using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.Calendars.Queries;
public sealed record GetCalendarByIdQuery(Guid Id)
    : IQuery<CalendarDto>;
