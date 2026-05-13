using CalendarApi.Contract;
using CalendarAPI.Application.Common.Repository;

namespace CalendarAPI.Application.EventTypes.Queries.Repositories;
public interface IEvenTypeQueryRepository
    :IQueryReposiotory
{
    Task<IReadOnlyCollection<EventTypeDto>> GetAvailableAsync(Guid userId, Guid calendarId, CancellationToken cancellationToken);
}
