using System.Text.Json.Serialization;

namespace CalendarApi.Contract;
public sealed record InvitationDto
{
    [JsonPropertyName("event_id")]
    public Guid EventId { get; init; }

    [JsonPropertyName("calendar_id")]
    public Guid CalendarId { get; init; }

    [JsonPropertyName("owner_user_id")]
    public Guid OwnerUserId { get; init; }

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

    [JsonPropertyName("status")]
    public string Status { get; init; } = default!;
}