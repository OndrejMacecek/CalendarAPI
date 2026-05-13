using CalendarApi.Contract;
using CalendarAPI.Application.Calendars.Queries.Repositories;
using CalendarAPI.Infrastructure.Calendars.Entities;
using CalendarAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.Calendars.QueryRepositories;
public sealed class CalendarQueryRepository
    : QueryRepositoryBase, ICalendarQueryRepository
{
    public CalendarQueryRepository(AppDbContext context)
        : base(context) { }

    public async Task<CalendarDto?> GetCalendarByIdAndUserAsync(Guid calendarId, Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Set<CalendarEntity>()
            .Where(x => x.Id == calendarId && x.UserId == userId).Select(x => new CalendarDto
            {
                Id = x.Id,
                Name = x.Name,
                TimeZoneId = x.TimeZoneId,
            }).FirstOrDefaultAsync();
    }
}
