using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Domain.Calendars.Repositories;

namespace CalendarAPI.Application.Calendars.UnitOfWorks;
public interface ICalendarUnitOfWork : IUnitOfWork
{
    ICalendarRepository Calendars { get; }
}