namespace CalendarAPI.Domain.Common;
public interface IRepository<TDomain>
    where TDomain : DomainBase
{
    Task AddAsync(TDomain domainObject, CancellationToken cancellationToken);

    Task<TDomain?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TDomain>> GetAllAsync(CancellationToken cancellationToken);

    Task UpdateAsync(TDomain domainObject, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}