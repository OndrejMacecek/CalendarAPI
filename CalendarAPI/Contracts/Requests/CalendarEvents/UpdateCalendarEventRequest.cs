using System.Text.Json.Serialization;

namespace CalendarAPI.Contracts.Requests.CalendarEvents;

public sealed record UpdateCalendarEventRequest
{
    [JsonPropertyName("event_type_id")]
    public Guid EventTypeId { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = default!;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("start_at_utc")]
    public DateTimeOffset StartAtUtc { get; init; }

    [JsonPropertyName("end_at_utc")]
    public DateTimeOffset EndAtUtc { get; init; }

    [JsonPropertyName("participant_ids")]
    public IReadOnlyCollection<Guid> ParticipantIds { get; init; } = [];
}
