namespace CalendarAPI.Domain.Common;

public abstract class DomainEntity
{
    public Guid Id { get; protected set; }

    protected DomainEntity(Guid id) { Id = id; }
}
