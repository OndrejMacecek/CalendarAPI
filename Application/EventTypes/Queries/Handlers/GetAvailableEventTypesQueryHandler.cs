using CalendarApi.Contract;
using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.EventTypes.Queries.Repositories;

namespace CalendarAPI.Application.EventTypes.Queries.Handlers;

public sealed class GetAvailableEventTypesQueryHandler
    : IQueryHandler<GetAvailableEventTypesQuery, IReadOnlyCollection<EventTypeDto>>
{
    private readonly IEvenTypeQueryRepository _query;
    private readonly ICurrentUser _currentUser;

    public GetAvailableEventTypesQueryHandler(
        IEvenTypeQueryRepository query,
        ICurrentUser currentUser)
    {
        _query = query;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyCollection<EventTypeDto>>> Handle(GetAvailableEventTypesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (userId == Guid.Empty)
        {
            return Result<IReadOnlyCollection<EventTypeDto>>.Failure(new("user.not_found", "User was not found."));
        }

        var eventTypes = await _query.GetAvailableAsync(userId, request.CalendarId, cancellationToken);

        return Result<IReadOnlyCollection<EventTypeDto>>.Success(eventTypes);
    }
}
