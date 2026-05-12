using CalendarApi.Contract;
using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;

namespace CalendarAPI.Application.Calendars.Queries.Handlers;
public sealed class GetCalendarEventsQueryHandler
    : IQueryHandler<GetCalendarEventsQuery, IReadOnlyCollection<CalendarEventDto>>
{
    private readonly ICalendarApiUnitOfWork _uow;
    private readonly ICurrentUser _currentUser;

    public GetCalendarEventsQueryHandler(
        ICalendarApiUnitOfWork uow,
        ICurrentUser currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyCollection<CalendarEventDto>>> Handle(
        GetCalendarEventsQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUser.UserId == Guid.Empty)
        {
            return Result<IReadOnlyCollection<CalendarEventDto>>.Failure(
                new Error("auth.user_missing", "Missing or invalid user header."));
        }

        var calendar = await _uow.Calendars.GetByIdAsync(            request.CalendarId,            cancellationToken);

        if (calendar is null || calendar.UserId != _currentUser.UserId)
        {
            return Result<IReadOnlyCollection<CalendarEventDto>>.Failure(
                new Error("calendar.not_found", "Calendar was not found."));
        }

        var events = await _uow.CalendarEvents.GetByCalendarIdInRangeAsync(
            request.CalendarId,
            request.FromUtc,
            request.ToUtc,
            cancellationToken);

        var dto = events
            .Select(x => new CalendarEventDto()
            {
                Id = x.Id,
                CalendarId = x.CalendarId,
                OwnerUserId = x.OwnerUserId,
                EventTypeId = x.EventTypeId,
                Title = x.Title,
                Description = x.Description,
                StartAtUtc = x.TimeRange.StartAtUtc,
                EndAtUtc = x.TimeRange.EndAtUtc,
                ParticipantIds = x.Participants.Select(p => p.UserId).ToList()
            }).ToList();

        return Result<IReadOnlyCollection<CalendarEventDto>>.Success(dto);
    }
}