using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.EventTypes.Commands;

public sealed record CreateEventTypeCommand(string Name, string? Color, int Priority, string Scope, Guid? CalendarId)
        : ICommand<Guid>;