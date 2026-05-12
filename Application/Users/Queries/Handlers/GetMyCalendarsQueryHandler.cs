using CalendarApi.Contract;
using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Domain.Calendars.Repositories;

namespace CalendarAPI.Application.Users.Queries.Handlers;
public sealed class GetMyCalendarsQueryHandler
    : IQueryHandler<GetMyCalendarsQuery, IReadOnlyCollection<CalendarDto>>
{
    private readonly ICalendarRepository _calendarRepository;
    private readonly ICurrentUser _currentUser;

    public GetMyCalendarsQueryHandler(
        ICalendarRepository calendarRepository,
        ICurrentUser currentUser)
    {
        _calendarRepository = calendarRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyCollection<CalendarDto>>> Handle(
        GetMyCalendarsQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUser.UserId == Guid.Empty)
        {
            return Result<IReadOnlyCollection<CalendarDto>>.Failure(new Error("auth.user_missing", "Missing or invalid user header."));
        }

        var calendars = await _calendarRepository.GetByUserIdAsync(_currentUser.UserId, cancellationToken);

        var dto = calendars
            .Select(x => new CalendarDto()
            {
                Id = x.Id,
                Name = x.Name,
                TimeZoneId = x.TimeZoneId
            }).ToList();

        return Result<IReadOnlyCollection<CalendarDto>>.Success(dto);
    }
}