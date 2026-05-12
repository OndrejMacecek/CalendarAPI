using System.Text.Json.Serialization;

namespace CalendarApi.Contract;


public record class CalendarDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;

    [JsonPropertyName("time_zone_id")]
    public string TimeZoneId { get; init; } = null!;
}

