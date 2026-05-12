using CalendarAPI.Domain.Users.Aggregates;
using CalendarAPI.Domain.Users.Repositories;
using CalendarAPI.Infrastructure.CalendarEvents;
using CalendarAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.Users.Repositories;
internal class UserRepository
    : BaseRepository<User, UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext context)
        : base(context) { }

    public async Task DeleteUserGraphAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await Context.Users
            .Include(x => x.Calendars)
                .ThenInclude(x => x.Events)
                    .ThenInclude(x => x.Participants)
        .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
            return;

        var participantRows = await Context.Set<EventParticipantEntity>()
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        Context.Set<EventParticipantEntity>().RemoveRange(participantRows);

        Context.Users.Remove(user);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var entity = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        return entity is null ? null : ToDomain(entity);
    }

    protected override void MapToExistingEntity(User domain, UserEntity entity)
    {
        entity.Email = domain.Email.Value;
        entity.DisplayName = domain.DisplayName;
    }

    protected override User ToDomain(UserEntity entity)
    {
        return User.Rehydrate(entity.Id, entity.Email, entity.DisplayName);
    }

    protected override UserEntity ToEntity(User domain)
    {
        return new UserEntity
        {
            Id = domain.Id,
            Email = domain.Email.Value,
            DisplayName = domain.DisplayName
        };
    }
}
