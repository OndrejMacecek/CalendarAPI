using CalendarAPI.Application.Calendars.Dtos;
using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.Queries;
public sealed record GetCalendarByIdQuery(Guid Id)
    : IQuery<CalendarDto>;
