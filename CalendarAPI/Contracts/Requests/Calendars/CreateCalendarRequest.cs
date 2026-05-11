using System.Text.Json.Serialization;

namespace CalendarAPI.Contracts.Requests.Calendars;

public sealed record CreateCalendarRequest
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = default!;
}