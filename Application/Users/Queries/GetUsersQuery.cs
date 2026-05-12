using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Contracts.Responses.Users;

namespace CalendarAPI.Application.Users.Queries;
public sealed record GetUsersQuery
    : IQuery<IReadOnlyCollection<UserDto>>;