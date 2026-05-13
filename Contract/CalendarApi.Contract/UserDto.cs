using CalendarApi.Contract;
using System.Text.Json.Serialization;

namespace CalendarAPI.Contracts.Responses.Users;

public class UserDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("email")]
    public string Email { get; init; } = default!;

    [JsonPropertyName("display_name")]
    public string DisplayName { get; init; } = default!;

    [JsonPropertyName("calendars")]
    public IReadOnlyCollection<CalendarDto> Calendars { get; init; } = [];
}