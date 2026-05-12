using CalendarApi.Contract;
using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Domain.Calendars.Repositories;

namespace CalendarAPI.Application.Calendars.Queries.Handlers;
public sealed class GetCalendarByIdQueryHandler
    : IQueryHandler<GetCalendarByIdQuery, CalendarDto>
{
    private readonly ICalendarRepository _calendarRepository;
    private readonly ICurrentUser _currentUser;

    public GetCalendarByIdQueryHandler(
        ICalendarRepository calendarRepository,
        ICurrentUser currentUser)
    {
        _calendarRepository = calendarRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<CalendarDto>> Handle(GetCalendarByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (userId == Guid.Empty)
        {
            return Result<CalendarDto>.Failure(new("user.not_found", "User was not found."));
        }

        var calendar = await _calendarRepository.GetByIdAsync(request.Id, cancellationToken);

        if (calendar == null)
        {
            return Result<CalendarDto>.Failure(new("dalendar.not_found", "Calendar wa not foound."));
        }

        return Result<CalendarDto>.Success(new CalendarDto() { Id = calendar.Id, Name = calendar.Name, TimeZoneId = calendar.TimeZoneId });
    }
}
