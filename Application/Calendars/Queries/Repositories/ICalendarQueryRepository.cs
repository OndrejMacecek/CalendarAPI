using CalendarApi.Contract;
using CalendarAPI.Application.Common.Repository;

namespace CalendarAPI.Application.Calendars.Queries.Repositories;
public interface ICalendarQueryRepository
    :IQueryReposiotory
{
    Task<CalendarDto?> GetCalendarByIdAndUserAsync(Guid calendarId, Guid userId, CancellationToken cancellationToken);
}
