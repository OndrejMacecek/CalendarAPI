using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.Users.Commands;
public sealed record CreateUserCommand(string email, string displayName)
        :ICommand<Guid>;