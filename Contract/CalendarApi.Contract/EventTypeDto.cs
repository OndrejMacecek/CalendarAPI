using CalendarAPI.Domain.EventTypes.Aggregates;
using System.Text.Json.Serialization;

namespace CalendarApi.Contract;

public record class EventTypeDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    [JsonPropertyName("scope")]
    public string Scope { get; init; } = default!;

    [JsonPropertyName("name")]
    public string Name { get; init; } = default!;

    [JsonPropertyName("color")]
    public string? Color { get; init; }

    [JsonPropertyName("priority")]
    public int Priority { get; init; }
}

