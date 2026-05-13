using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Users.Queries.Repositories;
using CalendarAPI.Contracts.Responses.Users;

namespace CalendarAPI.Application.Users.Queries.Handlers;

public sealed class GetUserByIdQueryHandler
    : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserQueryRepository _userQuery;

    public GetUserByIdQueryHandler(
        IUserQueryRepository userQuery)
    {
        _userQuery = userQuery;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userQuery.GetUserByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            return Result<UserDto>.Failure(new("user.not_found", "User was not found."));
        }

        return Result<UserDto>.Success(user);
    }
}
