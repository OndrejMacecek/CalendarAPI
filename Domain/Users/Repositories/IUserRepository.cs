using CalendarAPI.Domain.Common;
using CalendarAPI.Domain.Users.Aggregates;

namespace CalendarAPI.Domain.Users.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task DeleteUserGraphAsync(Guid userId, CancellationToken cancellationToken);
}