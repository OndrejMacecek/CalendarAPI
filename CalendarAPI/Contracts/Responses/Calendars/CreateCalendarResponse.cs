using System.Text.Json.Serialization;

namespace CalendarAPI.Contracts.Responses.Calendars;

public class CreateCalendarResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
}