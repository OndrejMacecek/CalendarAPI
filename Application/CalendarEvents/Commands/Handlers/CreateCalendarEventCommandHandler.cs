using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;
using CalendarAPI.Domain.CalendarEvents.Aggregates;
using CalendarAPI.Domain.EventTypes.Aggregates;

namespace CalendarAPI.Application.CalendarEvents.Commands.Handlers;
public sealed class CreateCalendarEventCommandHandler
    : ICommandHandler<CreateCalendarEventCommand, Guid>
{
    private readonly ICalendarApiUnitOfWork _uow;
    private readonly ICurrentUser _currentUser;

    public CreateCalendarEventCommandHandler(
        ICalendarApiUnitOfWork uow,
        ICurrentUser currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(CreateCalendarEventCommand request, CancellationToken cancellationToken)
    {
        var ownerUserId = _currentUser.UserId;

        if (ownerUserId == Guid.Empty)
        {
            return Result<Guid>.Failure(new Error("auth.user_missing", "Missing or invalid user header."));
        }

        var calendar = await _uow.Calendars.GetByIdAsync(request.CalendarId, cancellationToken);

        if (calendar is null || calendar.UserId != ownerUserId)
        {
            return Result<Guid>.Failure(new Error("calendar.not_found", "Calendar was not found."));
        }

        var eventType = await _uow.EventTypes.GetByIdAsync(request.EventTypeId, cancellationToken);

        if (eventType is null)
        {
            return Result<Guid>.Failure(new Error("event_type.not_found", "Event type was not found."));
        }

        var eventTypeAllowed =
            eventType.Scope == EventTypeScope.System ||
            eventType.Scope == EventTypeScope.User && eventType.UserId == ownerUserId ||
            eventType.Scope == EventTypeScope.Calendar && eventType.UserId == ownerUserId && eventType.CalendarId == request.CalendarId;

        if (!eventTypeAllowed)
        {
            return Result<Guid>.Failure(new Error("event_type.not_available", "Event type is not available for this calendar."));
        }

        var allUserIds = request.ParticipantIds
            .Append(ownerUserId)
            .Distinct()
            .ToList();

        foreach (var userId in allUserIds)
        {
            var hasConflict = await _uow.CalendarEvents.HasConflictAsync(
                userId,
                request.StartAtUtc,
                request.EndAtUtc,
                ignoredEventId: null,
                cancellationToken);

            if (hasConflict)
            {
                return Result<Guid>.Failure(new Error("event.time_conflict", $"User {userId} has a conflicting event."));
            }
        }

        var eventResult = CalendarEvent.Create(
            request.CalendarId,
            ownerUserId,
            request.EventTypeId,
            request.Title,
            request.Description,
            request.StartAtUtc.ToUniversalTime(),
            request.EndAtUtc.ToUniversalTime());

        if (eventResult.IsFailure)
        {
            return eventResult.Error!.ToApplicationFailure<Guid>();
        }

        var calendarEvent = eventResult.Value!;

        foreach (var participantId in request.ParticipantIds.Distinct())
        {
            if (participantId == ownerUserId)
            {
                continue;
            }

            var participant = await _uow.Users.GetByIdAsync(participantId, cancellationToken);

            if (participant is null)
            {
                return Result<Guid>.Failure(new Error("participant.not_found", $"Participant {participantId} was not found."));
            }

            var inviteResult = calendarEvent.InviteParticipant(participantId);

            if (inviteResult.IsFailure)
            {
                return inviteResult.Error!.ToApplicationFailure<Guid>();
            }
        }

        await _uow.BeginTransactionAsync(cancellationToken);

        try
        {
            await _uow.CalendarEvents.AddAsync(calendarEvent, cancellationToken);
            await _uow.CommitTransactionAsync(cancellationToken);

            return Result<Guid>.Success(calendarEvent.Id);
        }
        catch
        {
            await _uow.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
