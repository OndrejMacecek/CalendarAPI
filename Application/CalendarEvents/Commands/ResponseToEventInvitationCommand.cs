using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.CalendarEvents.Commands;
public sealed record ResponseToEventInvitationCommand(
    Guid EventId, string Status)
    : ICommand;