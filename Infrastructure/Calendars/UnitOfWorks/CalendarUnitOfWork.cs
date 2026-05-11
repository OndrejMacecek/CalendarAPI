using CalendarAPI.Application.Calendars.UnitOfWorks;
using CalendarAPI.Domain.Calendars.Repositories;
using CalendarAPI.Infrastructure.Persistence;

namespace CalendarAPI.Infrastructure.Calendars.UnitOfWorks;
public sealed class CalendarUnitOfWork
    : UnitOfWork, ICalendarUnitOfWork
{
    public CalendarUnitOfWork(
        AppDbContext context,
        ICalendarRepository calendars)
        : base(context)
    {
        Calendars = calendars;
    }

    public ICalendarRepository Calendars { get; }
}