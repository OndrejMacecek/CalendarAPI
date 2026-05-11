namespace CalendarAPI.Infrastructure.Persistence.Entities;

public abstract class DbEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public DateTimeOffset UpdatedAtUtc { get; set; }
}