using CalendarApi.Contract;
using CalendarAPI.Application.Calendars.Queries.Repositories;
using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;

namespace CalendarAPI.Application.Calendars.Queries.Handlers;
public sealed class GetCalendarByIdQueryHandler
    : IQueryHandler<GetCalendarByIdQuery, CalendarDto>
{
    private readonly ICalendarQueryRepository _query;
    private readonly ICurrentUser _currentUser;

    public GetCalendarByIdQueryHandler(
        ICalendarQueryRepository query,
        ICurrentUser currentUser)
    {
        _query = query;
        _currentUser = currentUser;
    }

    public async Task<Result<CalendarDto>> Handle(GetCalendarByIdQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.UserId;

        if (currentUserId == Guid.Empty)
        {
            return Result<CalendarDto>.Failure(new("user.not_found", "User was not found."));
        }

        var calendar = await _query.GetCalendarByIdAndUserAsync(request.Id, currentUserId, cancellationToken);

        if (calendar == null)
        {
            return Result<CalendarDto>.Failure(new("dalendar.not_found", "Calendar wa not foound."));
        }

        return Result<CalendarDto>.Success(calendar);
    }
}
