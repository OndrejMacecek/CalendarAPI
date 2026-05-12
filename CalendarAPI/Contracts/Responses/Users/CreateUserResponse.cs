using System.Text.Json.Serialization;

namespace CalendarAPI.Contracts.Responses.Users;

public sealed record CreateUserResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
}
    
