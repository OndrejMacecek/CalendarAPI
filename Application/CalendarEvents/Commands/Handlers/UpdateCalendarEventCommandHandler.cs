using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;
using MediatR;

namespace CalendarAPI.Application.CalendarEvents.Commands.Handlers;
public sealed class UpdateCalendarEventCommandHandler
    : IRequestHandler<UpdateCalendarEventCommand, Result>
{
    private readonly ICalendarApiUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdateCalendarEventCommandHandler(
        ICalendarApiUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateCalendarEventCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.UserId;

        var calendarEvent = await _unitOfWork.CalendarEvents.GetByIdAsync(
            request.EventId,
            cancellationToken);

        if (calendarEvent is null)
        {
            return Result.Failure(new("event.not_fount", "Event not found."));
        }

        if (calendarEvent.OwnerUserId != currentUserId)
        {
            return Result.Failure(new("event_conflict", "Only event owner can update the event."));
        }

        var participantIds = request.ParticipantIds
            .Distinct()
            .Where(id => id != currentUserId)
            .ToArray();

        foreach (var participantId in participantIds)
        {
            var participantExists = await _unitOfWork.Users.ExistsAsync(participantId, cancellationToken);

            if (!participantExists)
            {
                return Result.Failure(new("user.not_found", $"Participant '{participantId}' does not exist."));
            }
        }

        var usersToCheck = participantIds
            .Append(currentUserId)
            .Distinct()
            .ToArray();

        foreach (var userId in usersToCheck)
        {
            var hasConflict = await _unitOfWork.CalendarEvents.HasConflictAsync(
                userId,
                request.StartAtUtc.UtcDateTime,
                request.EndAtUtc.UtcDateTime,
                ignoredEventId: calendarEvent.Id,
                cancellationToken);

            if (hasConflict)
            {
                return Result.Failure(new("user.conflicts", $"User '{userId}' has conflicting event."));
            }
        }

        var updateResult = calendarEvent.Update(
            request.EventTypeId,
            request.Title,
            request.Description,
            request.StartAtUtc.ToUniversalTime(),
            request.EndAtUtc.ToUniversalTime(),
            participantIds);

        if (updateResult.IsFailure)
        {
            return updateResult.Error!.ToApplicationFailure();
        }
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _unitOfWork.CalendarEvents.UpdateAsync(calendarEvent, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }


        return Result.Success();
    }
}