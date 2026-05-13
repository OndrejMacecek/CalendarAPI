using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Users.Queries.Repositories;
using CalendarAPI.Contracts.Responses.Users;

namespace CalendarAPI.Application.Users.Queries.Handlers;

public sealed class GetUsersQueryHandler
    : IQueryHandler<GetUsersQuery, IReadOnlyCollection<UserDto>>
{
    private readonly IUserQueryRepository _userQuery;

    public GetUsersQueryHandler(
        IUserQueryRepository userQuery)
    {
        _userQuery = userQuery;
    }

    public async Task<Result<IReadOnlyCollection<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userQuery.GetAllUsersdAsync(cancellationToken);
        return Result<IReadOnlyCollection<UserDto>>.Success(users);
    }
}
