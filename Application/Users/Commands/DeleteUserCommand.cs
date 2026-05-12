
using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.Users.Commands;
public sealed record DeleteUserCommand(Guid UserId)
    : ICommand;