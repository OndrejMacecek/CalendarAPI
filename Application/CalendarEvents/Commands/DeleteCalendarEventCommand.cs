
using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.CalendarEvents.Commands;

public sealed record DeleteCalendarEventCommand(Guid EventId) 
    : ICommand;