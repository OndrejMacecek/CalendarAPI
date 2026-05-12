using CalendarApi.Contract;
using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;

namespace CalendarAPI.Application.Calendars.Queries.Handlers;

public sealed class GetMyEvensQueryHandler
    : IQueryHandler<GetMyEventsQuery, IReadOnlyCollection<CalendarEventDto>>
{
    private readonly ICalendarApiUnitOfWork _uow;
    private readonly ICurrentUser _currentUser;

    public GetMyEvensQueryHandler(
        ICalendarApiUnitOfWork uow,
        ICurrentUser currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyCollection<CalendarEventDto>>> Handle(
        GetMyEventsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (userId == Guid.Empty)
        {
            return Result<IReadOnlyCollection<CalendarEventDto>>.Failure(
                new Error("auth.user_missing", "Missing or invalid user header."));
        }


        var events = await _uow.CalendarEvents.GetVisibleForUserInRangeAsync(
            userId,
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