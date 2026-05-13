using CalendarAPI.Domain.Common;
using CalendarAPI.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.Persistence;
public abstract class BaseRepository<TDomain, TEntity>
    : IRepository<TDomain>
    where TDomain : DomainEntity
    where TEntity : DbEntity
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public async Task AddAsync(TDomain domain, CancellationToken cancellationToken)
    {
        var entity = ToEntity(domain);
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async Task<TDomain?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? null : ToDomain(entity);
    }

    /// <summary>
    /// ONLY for testing!!!!
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IReadOnlyCollection<TDomain>> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await DbSet.AsNoTracking().ToListAsync(cancellationToken);
        return entities.Select(ToDomain).ToList();
    }

    public virtual async Task UpdateAsync(TDomain domain, CancellationToken cancellationToken)
    {
        var entity = await DbSet.FirstOrDefaultAsync(x => x.Id == domain.Id, cancellationToken);

        if (entity is null)
        {
            return;
        }

        MapToExistingEntity(domain, entity);
    }

    public Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return DbSet.AnyAsync(x => x.Id == userId, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await DbSet
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return;
        }

        DbSet.Remove(entity);
    }

    protected abstract TEntity ToEntity(TDomain domain);

    protected abstract TDomain ToDomain(TEntity entity);

    protected abstract void MapToExistingEntity(TDomain domain, TEntity entity);
}