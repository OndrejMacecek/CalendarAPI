using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Contracts.Responses.Users;
using CalendarAPI.Domain.Users.Repositories;

namespace CalendarAPI.Application.Users.Queries.Handlers;

public sealed class GetUsersQueryHandler
    : IQueryHandler<GetUsersQuery, IReadOnlyCollection<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IReadOnlyCollection<UserDto>>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        var result = users
            .Select(x =>
                new UserDto
                {
                    Id = x.Id,
                    DisplayName = x.DisplayName,
                    Email = x.Email.Value,
                }
                ).ToList();

        return Result<IReadOnlyCollection<UserDto>>.Success(result);
    }
}
