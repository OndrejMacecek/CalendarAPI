using CalendarAPI.Domain.Common;

namespace CalendarAPI.Domain.Calendars.Aggregates;
public sealed class Calendar
    : DomainAggregate
{
    public Guid UserId { get; private set; }

    public string Name { get; private set; } = null!;
    public string TimeZoneId { get; private set; } = null!;

    private Calendar(Guid id)
        : base(id) { }

    public static DomainResult<Calendar> Create(
        Guid userId,
        string name,
        string timeZoneId = "UTC",
        Guid? id = null)
    {
        if (userId == Guid.Empty)
        {
            return DomainResult<Calendar>.Failure(new DomainError("calendar.user_required", "Calendar user is required."));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return DomainResult<Calendar>.Failure(new("calendar.name_required", "Calendar name is required."));
        }

        if (name.Length > 100)
        {
            return DomainResult<Calendar>.Failure(new("calendar.name_too_long", "Calendar name must be 100 characters or less."));
        }

        if (string.IsNullOrWhiteSpace(timeZoneId))
        {
            return DomainResult<Calendar>.Failure(new("calendar.invalid_time_zone", "Calendar time zone is invalid."));
        }

        return DomainResult<Calendar>.Success(
            new Calendar(id ?? Guid.NewGuid())
            {
                UserId = userId,
                Name = name,
                TimeZoneId = timeZoneId
            });
    }

    public static Calendar Rehydrate(
        Guid id,
        Guid userId,
        string name,
        string timeZoneId)
    {
        return new Calendar(id)
        {
            UserId = userId,
            Name = name,
            TimeZoneId = timeZoneId
        };
    }

    public DomainResult Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return DomainResult.Failure(new("calendar.name_required", "Calendar name is required."));
        }

        if (name.Length > 100)
        {
            return DomainResult.Failure(new("calendar.name_too_long", "Calendar name must be 100 characters or less."));
        }

        Name = name.Trim();

        return DomainResult.Success();
    }
}