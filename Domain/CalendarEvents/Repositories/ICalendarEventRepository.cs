using CalendarAPI.Domain.CalendarEvents.Aggregates;
using CalendarAPI.Domain.Common;

namespace CalendarAPI.Domain.CalendarEvents.Repositories;

public interface ICalendarEventRepository
    : IRepository<CalendarEvent>
{
    Task<IReadOnlyCollection<CalendarEvent>> GetByCalendarIdInRangeAsync(
        Guid calendarId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken cancellationToken);

    Task<bool> HasConflictAsync(
        Guid userId, DateTimeOffset startAtUtc, DateTimeOffset endAtUtc, Guid? ignoredEventId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<CalendarEvent>> GetInvitationsForUserAsync(
        Guid userId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<CalendarEvent>> GetVisibleForUserInRangeAsync(
        Guid userId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken cancellationToken);
}
