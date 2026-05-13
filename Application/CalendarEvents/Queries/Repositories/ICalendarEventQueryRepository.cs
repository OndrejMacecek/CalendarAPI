using CalendarApi.Contract;
using CalendarAPI.Application.Common.Repository;
using CalendarAPI.Domain.CalendarEvents.Aggregates;

namespace CalendarAPI.Application.CalendarEvents.Queries.Repositories;
public interface ICalendarEventQueryRepository
    :IQueryReposiotory
{
    Task<IReadOnlyCollection<CalendarEventDto>> GetByCalendarIdInRangeAsync(
        Guid calendarId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<CalendarEventDto>> GetVisibleForUserInRangeAsync(
        Guid userId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<InvitationDto>> GetInvitationsForUserAsync(
        Guid userId, CancellationToken cancellationToken);
}
