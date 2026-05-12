using System.Text.Json.Serialization;

namespace CalendarAPI.Contracts.Requests.Users;

public sealed record CreateUserRequest
{
    [JsonPropertyName("email")]
    public string Email { get; init; } = default!;
    [JsonPropertyName("display_name")]
    public string DisplayName { get; init; } = default!;
}