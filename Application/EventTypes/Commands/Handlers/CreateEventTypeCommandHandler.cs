using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;
using CalendarAPI.Domain.EventTypes.Aggregates;

namespace CalendarAPI.Application.EventTypes.Commands.Handlers;

public sealed class CreateEventTypeCommandHandler
    : ICommandHandler<CreateEventTypeCommand, Guid>
{
    private readonly ICalendarApiUnitOfWork _uow;
    private readonly ICurrentUser _currentUser;

    public CreateEventTypeCommandHandler(
        ICalendarApiUnitOfWork uow,
        ICurrentUser currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(CreateEventTypeCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (userId == Guid.Empty)
        {
            return Result<Guid>.Failure(
                new Error("auth.user_missing", "Missing or invalid user header."));
        }

        if (!Enum.TryParse<EventTypeScope>(request.Scope, true, out var scope))
        {
            return Result<Guid>.Failure(
                new Error("event_type.not_foubt", "Event type doesnt exists. (system, user, calendar)"));
        }

        if (scope == EventTypeScope.Calendar)
        {
            var calendar = await _uow.Calendars.GetByIdAsync(request.CalendarId!.Value, cancellationToken);

            if (calendar is null || calendar.UserId != userId)
            {
                return Result<Guid>.Failure(
                    new Error("calendar.not_found", "Calendar was not found."));
            }
        }

        var eventTypeResult = EventType.Create(
            scope,
            userId,
            request.CalendarId,
            request.Name,
            request.Color,
            request.Priority);

        if (eventTypeResult.IsFailure)
        {
            return eventTypeResult.Error!.ToApplicationFailure<Guid>();
        }

        await _uow.BeginTransactionAsync(cancellationToken);
        await _uow.EventTypes.AddAsync(eventTypeResult.Value!, cancellationToken);
        await _uow.CommitTransactionAsync(cancellationToken);

        return Result<Guid>.Success(eventTypeResult.Value!.Id);
    }
}