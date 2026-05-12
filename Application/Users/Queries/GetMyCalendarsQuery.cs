using CalendarApi.Contract;
using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.Users.Queries;
public sealed record GetMyCalendarsQuery
    : IQuery<IReadOnlyCollection<CalendarDto>>;
