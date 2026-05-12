using CalendarAPI.Domain.Common;

namespace CalendarAPI.Domain.Calendars.ValueObjects;
public sealed class EventTimeRange
    : ValueObject
{
    public DateTimeOffset StartAtUtc { get; }
    public DateTimeOffset EndAtUtc { get; }

    private EventTimeRange(DateTimeOffset startAtUtc, DateTimeOffset endAtUtc)
    {
        StartAtUtc = startAtUtc;
        EndAtUtc = endAtUtc;
    }

    public static DomainResult<EventTimeRange> Create(
        DateTimeOffset startAtUtc,
        DateTimeOffset endAtUtc)
    {
        if (endAtUtc <= startAtUtc)
        {
            return DomainResult<EventTimeRange>.Failure(new("event.invalid_time_range", "Event end must be after event start."));
        }

        return DomainResult<EventTimeRange>.Success(
            new EventTimeRange(startAtUtc, endAtUtc));
    }

    public bool Overlaps(EventTimeRange other)
    {
        return StartAtUtc < other.EndAtUtc &&
               EndAtUtc > other.StartAtUtc;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StartAtUtc;
        yield return EndAtUtc;
    }
}