using CalendarApi.Contract;
using CalendarAPI.Application.CalendarEvents.Queries.Repositories;
using CalendarAPI.Domain.CalendarEvents.Entities;
using CalendarAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.CalendarEvents.QueryRepositories;
public class CalendarEventQueryRepository
    : QueryRepositoryBase, ICalendarEventQueryRepository
{
    public CalendarEventQueryRepository(AppDbContext context)
        : base(context) { }

    public async Task<IReadOnlyCollection<CalendarEventDto>> GetByCalendarIdInRangeAsync(
        Guid calendarId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken cancellationToken)
    {
        var from = fromUtc.UtcDateTime;
        var to = toUtc.UtcDateTime;

        return await Context.Set<CalendarEventEntity>()
            .AsNoTracking()
            .Where(x =>
                x.CalendarId == calendarId &&
                x.StartAtUtc < to &&
                x.EndAtUtc > from)
            .Select(x => new CalendarEventDto
            {
                CalendarId = x.CalendarId,
                Description = x.Description,
                EndAtUtc = x.EndAtUtc,
                EventTypeId = x.EventTypeId,
                Id = x.Id,
                OwnerUserId = x.OwnerUserId,
                ParticipantIds = x.Participants.Select(p => p.UserId).ToList(),
                StartAtUtc = x.StartAtUtc,
                Title = x.Title
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CalendarEventDto>> GetVisibleForUserInRangeAsync(
        Guid userId, DateTimeOffset fromUtc, DateTimeOffset toUtc, CancellationToken cancellationToken)
    {
        var from = fromUtc.UtcDateTime;
        var to = toUtc.UtcDateTime;

        return await Context.Set<CalendarEventEntity>()
            .AsNoTracking()
            .Where(x =>
                x.StartAtUtc < to &&
                x.EndAtUtc > from &&
                (
                    x.OwnerUserId == userId ||
                    x.Participants.Any(p =>
                        p.UserId == userId &&
                        p.Status != EventParticipantStatus.Declined)
                ))
            .Select(x => new CalendarEventDto()
            {
                Id = x.Id,
                CalendarId = x.CalendarId,
                OwnerUserId = x.OwnerUserId,
                EventTypeId = x.EventTypeId,
                Title = x.Title,
                Description = x.Description,
                StartAtUtc = x.StartAtUtc,
                EndAtUtc = x.EndAtUtc,
                ParticipantIds = x.Participants.Select(p => p.UserId).ToList()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<InvitationDto>> GetInvitationsForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Set<EventParticipantEntity>()
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => new InvitationDto
            {
                EventId = p.EventId,
                CalendarId = p.Event.Id,
                OwnerUserId = p.Event.OwnerUserId,
                EventTypeId = p.Event.EventTypeId,
                Title = p.Event.Title,
                Description = p.Event.Description,
                StartAtUtc = p.Event.StartAtUtc,
                EndAtUtc = p.Event.EndAtUtc,
                Status = p.Status.ToString()
            })
            .ToListAsync(cancellationToken);
    }
}
