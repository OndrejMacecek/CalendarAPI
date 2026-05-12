using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Domain.CalendarEvents.Repositories;
using CalendarAPI.Domain.Calendars.Repositories;
using CalendarAPI.Domain.EventTypes.Repositories;
using CalendarAPI.Domain.Users.Repositories;

namespace CalendarAPI.Application.Common.UnitOfWork;

public interface ICalendarApiUnitOfWork : IUnitOfWork
{
    IUserRepository Users { get; }
    ICalendarRepository Calendars { get; }
    ICalendarEventRepository CalendarEvents { get; }
    IEventTypeRepository EventTypes { get; }
}