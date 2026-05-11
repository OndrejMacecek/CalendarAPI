namespace CalendarAPI.Infrastructure.Common;
public interface ITimeProvider
{
    DateTimeOffset UtcNow { get; }
}

public class SnapshotTimeProvider
    : ITimeProvider
{
    private readonly DateTimeOffset _utcNow = DateTimeOffset.UtcNow;

    public DateTimeOffset UtcNow => _utcNow;
}
