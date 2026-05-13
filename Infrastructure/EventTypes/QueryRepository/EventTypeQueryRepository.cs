using CalendarApi.Contract;
using CalendarAPI.Application.EventTypes.Queries.Repositories;
using CalendarAPI.Domain.EventTypes.Aggregates;
using CalendarAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.EventTypes.QueryRepository;

internal class EventTypeQueryRepository
    : QueryRepositoryBase, IEvenTypeQueryRepository
{
    public EventTypeQueryRepository(AppDbContext context)
        : base(context) { }

    public async Task<IReadOnlyCollection<EventTypeDto>> GetAvailableAsync(Guid userId, Guid calendarId, CancellationToken cancellationToken)
    {
        return await Context.Set<EventTypeEntity>().AsNoTracking()
           .Where(x =>
               x.Scope == EventTypeScope.System ||
               x.Scope == EventTypeScope.User && x.UserId == userId ||
               x.Scope == EventTypeScope.Calendar &&
               x.UserId == userId &&
               x.CalendarId == calendarId)
           .Select(x => new EventTypeDto
           {
               Id = x.Id,
               Color = x.Color,
               Name = x.Name,
               Priority = x.Priority,
               Scope = x.Scope.ToString(),
               CalendarId = x.CalendarId,
               UserId = x.UserId

           }).ToListAsync(cancellationToken);
    }
}
