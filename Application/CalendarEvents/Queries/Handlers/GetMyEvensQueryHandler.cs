using CalendarApi.Contract;
using CalendarAPI.Application.CalendarEvents.Queries.Repositories;
using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;

namespace CalendarAPI.Application.CalendarEvents.Queries.Handlers;

public sealed class GetMyEvensQueryHandler
    : IQueryHandler<GetMyEventsQuery, IReadOnlyCollection<CalendarEventDto>>
{
    private readonly ICalendarEventQueryRepository _query;
    private readonly ICurrentUser _currentUser;

    public GetMyEvensQueryHandler(
        ICalendarEventQueryRepository query,
        ICurrentUser currentUser)
    {
        _query = query;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyCollection<CalendarEventDto>>> Handle(GetMyEventsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (userId == Guid.Empty)
        {
            return Result<IReadOnlyCollection<CalendarEventDto>>.Failure(
                new Error("auth.user_missing", "Missing or invalid user header."));
        }

        var events = await _query.GetVisibleForUserInRangeAsync(
            userId,
            request.FromUtc,
            request.ToUtc,
            cancellationToken);

        return Result<IReadOnlyCollection<CalendarEventDto>>.Success(events);
    }
}