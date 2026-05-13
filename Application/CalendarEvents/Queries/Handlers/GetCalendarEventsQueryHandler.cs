using CalendarApi.Contract;
using CalendarAPI.Application.CalendarEvents.Queries.Repositories;
using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;

namespace CalendarAPI.Application.CalendarEvents.Queries.Handlers;

public sealed class GetCalendarEventsQueryHandler
    : IQueryHandler<GetCalendarEventsQuery, IReadOnlyCollection<CalendarEventDto>>
{
    private readonly ICalendarEventQueryRepository _query;
    private readonly ICalendarApiUnitOfWork _uow;
    private readonly ICurrentUser _currentUser;

    public GetCalendarEventsQueryHandler(
        ICalendarEventQueryRepository query,
        ICalendarApiUnitOfWork uow,
        ICurrentUser currentUser)
    {
        _query = query;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyCollection<CalendarEventDto>>> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUser.UserId;
        if (_currentUser.UserId == Guid.Empty)
        {
            return Result<IReadOnlyCollection<CalendarEventDto>>.Failure(
                new Error("auth.user_missing", "Missing or invalid user header."));
        }

        var calendar = await _uow.Calendars.GetByIdAsync(request.CalendarId, cancellationToken);

        if (calendar is null || calendar.UserId != _currentUser.UserId)
        {
            return Result<IReadOnlyCollection<CalendarEventDto>>.Failure(
                new Error("calendar.not_found", "Calendar was not found."));
        }

        var events = await _query.GetByCalendarIdInRangeAsync(
            calendar.Id,
            request.FromUtc,
            request.ToUtc,
            cancellationToken);

        return Result<IReadOnlyCollection<CalendarEventDto>>.Success(events);
    }
}