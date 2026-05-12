using CalendarAPI.Application.Common.UnitOfWork;
using CalendarAPI.Domain.CalendarEvents.Repositories;
using CalendarAPI.Domain.Calendars.Repositories;
using CalendarAPI.Domain.EventTypes.Repositories;
using CalendarAPI.Domain.Users.Repositories;
using CalendarAPI.Infrastructure.Persistence;

namespace CalendarAPI.Infrastructure.Common;

public sealed class CalendarApiUnitOfWork
    : UnitOfWork, ICalendarApiUnitOfWork
{
    public CalendarApiUnitOfWork(
        AppDbContext context,
        ICalendarRepository calendars,
        IUserRepository user,
        ICalendarEventRepository calendarEvent,
        IEventTypeRepository eventType
        )
        : base(context)
    {
        Calendars = calendars;
        Users = user;
        CalendarEvents = calendarEvent;
        EventTypes = eventType;
    }

    public ICalendarRepository Calendars { get; }

    public IUserRepository Users { get; }

    public ICalendarEventRepository CalendarEvents { get; }

    public IEventTypeRepository EventTypes { get; }
}