using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Contracts.Responses.Users;
using CalendarAPI.Domain.Users.Repositories;

namespace CalendarAPI.Application.Users.Queries.Handlers;

public sealed class GetUserByIdQueryHandler
    : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
        {
            return Result<UserDto>.Failure(new("user.not_found", "User was not found."));
        }

        return Result<UserDto>.Success(
            new UserDto
            {
                Id = request.Id,
                DisplayName = user.DisplayName,
                Email = user.Email.Value
            });
    }
}
