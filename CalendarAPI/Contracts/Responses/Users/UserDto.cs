using CalendarAPI.Application.Common.Messaging;
using System.Text.Json.Serialization;

namespace CalendarAPI.Contracts;

public class UserDto
{
    public Guid Id { get; init; }

    public string Email { get; init; } = default!;

    public string DisplayName { get; init; } = default!;
}