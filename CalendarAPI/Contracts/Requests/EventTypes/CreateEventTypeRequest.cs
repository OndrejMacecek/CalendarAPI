using CalendarAPI.Domain.EventTypes.Aggregates;
using System.Text.Json.Serialization;

namespace CalendarAPI.Contracts.Requests.EventTypes;

public sealed record CreateEventTypeRequest
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = default!;

    [JsonPropertyName("color")]
    public string? Color { get; init; }

    [JsonPropertyName("priority")]
    public int Priority { get; init; }

    [JsonPropertyName("scope")]
    public string Scope { get; init; } = default!;

    [JsonPropertyName("calendar_id")]
    public Guid? CalendarId { get; init; }
}