using CalendarAPI.Domain.Calendars.Entities;
using CalendarAPI.Domain.Calendars.Repositories;
using CalendarAPI.Infrastructure.Calendars.Entities;
using CalendarAPI.Infrastructure.Persistence;

namespace CalendarAPI.Infrastructure.Calendars.Repositories;
public sealed class CalendarRepository
    : BaseRepository<Calendar, CalendarEntity>, ICalendarRepository
{
    public CalendarRepository(AppDbContext context)
        : base(context)
    {
    }

    protected override CalendarEntity ToEntity(Calendar domainObject)
    {
        return new CalendarEntity
        {
            Id = domainObject.Id,
            Name = domainObject.Name
        };
    }

    protected override Calendar ToDomain(CalendarEntity entity)
    {
        return Calendar.Rehydrate(
            entity.Id,
            entity.Name);
    }

    protected override void MapToExistingEntity(Calendar domainObject, CalendarEntity entity)
    {
        entity.Name = domainObject.Name;
    }
}