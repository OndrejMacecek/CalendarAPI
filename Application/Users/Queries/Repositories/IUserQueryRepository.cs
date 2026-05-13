using CalendarAPI.Application.Common.Repository;
using CalendarAPI.Contracts.Responses.Users;

namespace CalendarAPI.Application.Users.Queries.Repositories;

public interface IUserQueryRepository
    :IQueryReposiotory
{
    Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<UserDto>> GetAllUsersdAsync(CancellationToken cancellationToken);
}
