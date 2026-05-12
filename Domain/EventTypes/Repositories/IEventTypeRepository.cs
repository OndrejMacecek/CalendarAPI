using CalendarAPI.Domain.Common;
using CalendarAPI.Domain.EventTypes.Aggregates;

namespace CalendarAPI.Domain.EventTypes.Repositories;

public interface IEventTypeRepository
    : IRepository<EventType>
{
    Task<IReadOnlyCollection<EventType>> GetAvailableAsync(Guid userId, Guid calendarId, CancellationToken cancellationToken);
}
