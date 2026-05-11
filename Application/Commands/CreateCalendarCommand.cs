using CalendarAPI.Application.Calendars.Dtos;
using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.Commands;
public sealed record CreateCalendarCommand(CalendarDto Calendar)
    : ICommand<Guid>;