using CalendarAPI.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CalendarAPI.Application.Common;
public sealed class CurrentUser 
    : ICurrentUser
{
    public Guid UserId { get; }

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var value = httpContextAccessor.HttpContext?.Request.Headers["X-User-Id"].FirstOrDefault();

        if (!Guid.TryParse(value, out var userId))
        {
            UserId = Guid.Empty;
            return;
        }

        UserId = userId;
    }
}