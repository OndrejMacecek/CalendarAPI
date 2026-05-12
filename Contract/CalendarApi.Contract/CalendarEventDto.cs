namespace CalendarApi.Contract;

public sealed record CalendarEventDto
{
    public Guid Id { get; init; }
    public Guid CalendarId { get; init; }
    public Guid OwnerUserId { get; init; }
    public Guid EventTypeId { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public DateTimeOffset StartAtUtc { get; init; }
    public DateTimeOffset EndAtUtc { get; init; }
    public IReadOnlyCollection<Guid> ParticipantIds { get; init; } = default!;
}