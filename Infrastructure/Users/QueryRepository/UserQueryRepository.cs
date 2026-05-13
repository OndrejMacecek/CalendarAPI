using CalendarApi.Contract;
using CalendarAPI.Application.Users.Queries.Repositories;
using CalendarAPI.Contracts.Responses.Users;
using CalendarAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.Users.QueryRepository;
public sealed class UserQueryRepository
    : QueryRepositoryBase, IUserQueryRepository
{
    public UserQueryRepository(AppDbContext context)
        : base(context) { }

    public async Task<IReadOnlyCollection<UserDto>> GetAllUsersdAsync(CancellationToken cancellationToken)
    {
        return await Context.Set<UserEntity>().AsNoTracking()
            .Select(x => new UserDto
            {
                Id = x.Id,
                DisplayName = x.DisplayName,
                Email = x.Email,
                Calendars = x.Calendars
                    .Select(c => new CalendarDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        TimeZoneId = c.TimeZoneId
                    })
                    .ToList()
            }).ToListAsync(cancellationToken);
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Set<UserEntity>().AsNoTracking()
        .Where(x => x.Id == userId)
        .Select(x => new UserDto
        {
            Id = x.Id,
            DisplayName = x.DisplayName,
            Email = x.Email,
            Calendars = x.Calendars
                .Select(c => new CalendarDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    TimeZoneId = c.TimeZoneId
                })
                .ToList()
        }).FirstOrDefaultAsync(cancellationToken);
    }
}
