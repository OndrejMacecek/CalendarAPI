using CalendarAPI.Domain.Calendars.Aggregates;
using CalendarAPI.Domain.Common;

namespace CalendarAPI.Domain.Calendars.Repositories;

public interface ICalendarRepository 
    : IRepository<Calendar>
{
    Task<IReadOnlyCollection<Calendar>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
