using CalendarAPI.Infrastructure.Calendars.Entities;
using CalendarAPI.Infrastructure.Persistence.Entities;

namespace CalendarAPI.Infrastructure.Users;
public sealed class UserEntity
    :DbEntity
{
    public string Email { get; set; } = default!;
    public string DisplayName { get; set; } = default!;

    public ICollection<CalendarEntity> Calendars { get; set; } = [];
}
