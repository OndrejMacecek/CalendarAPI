using CalendarApi.Contract;
using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Contracts.Responses.Users;
using CalendarAPI.Domain.EventTypes.Repositories;

namespace CalendarAPI.Application.EventTypes.Queries.Handlers;

public sealed class GetAvailableEventTypesQueryHandler
    : IQueryHandler<GetAvailableEventTypesQuery, IReadOnlyCollection<EventTypeDto>>
{
    private readonly IEventTypeRepository _eventTypeRepository;
    private readonly ICurrentUser _currentUser;

    public GetAvailableEventTypesQueryHandler(
        IEventTypeRepository eventTypeRepository,
        ICurrentUser currentUser)
    {
        _eventTypeRepository = eventTypeRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyCollection<EventTypeDto>>> Handle(GetAvailableEventTypesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (userId == Guid.Empty)
        {
            return Result<IReadOnlyCollection<EventTypeDto>>.Failure(new("user.not_found", "User was not found."));
        }

        var eventTypes = await _eventTypeRepository.GetAvailableAsync(userId, request.CalendarId, cancellationToken);

        return Result<IReadOnlyCollection<EventTypeDto>>.Success(
            eventTypes.Select(x => new EventTypeDto()
            {
                Color = x.Color,
                Name = x.Name,
                Priority = x.Priority,
                Scope = x.Scope.ToString(),
                Id = x.Id
            }).ToList());
    }
}
