using CalendarAPI.Domain.Calendars.Aggregates;
using CalendarAPI.Domain.Calendars.Repositories;
using CalendarAPI.Infrastructure.Calendars.Entities;
using CalendarAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.Calendars.Repositories;
public sealed class CalendarRepository
    : BaseRepository<Calendar, CalendarEntity>, ICalendarRepository
{
    public CalendarRepository(AppDbContext context)
        : base(context) { }

    public async Task<IReadOnlyCollection<Calendar>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var entities = await DbSet.AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        return entities.Select(ToDomain).ToList();
    }

    protected override CalendarEntity ToEntity(Calendar domain)
    {
        return new CalendarEntity
        {
            Id = domain.Id,
            UserId = domain.UserId,
            Name = domain.Name,
            TimeZoneId = domain.TimeZoneId
        };
    }

    protected override Calendar ToDomain(CalendarEntity entity)
    {
        return Calendar.Rehydrate(
            entity.Id,
            entity.UserId,
            entity.Name,
            entity.TimeZoneId);
    }

    protected override void MapToExistingEntity(Calendar domain, CalendarEntity entity)
    {
        entity.UserId = domain.UserId;
        entity.Name = domain.Name;
        entity.TimeZoneId = domain.TimeZoneId;
    }
}