using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;
using CalendarAPI.Domain.CalendarEvents.Entities;
using CalendarAPI.Domain.Common;

namespace CalendarAPI.Application.CalendarEvents.Commands.Handlers;

public sealed class RespondToEventInvitationCommandHandler
    : ICommandHandler<RespondToEventInvitationCommand>
{
    private readonly ICalendarApiUnitOfWork _uow;
    private readonly ICurrentUser _currentUser;

    public RespondToEventInvitationCommandHandler(
        ICalendarApiUnitOfWork uow,
        ICurrentUser currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(RespondToEventInvitationCommand request, CancellationToken cancellationToken)
    {
        var calendarEvent = await _uow.CalendarEvents.GetByIdAsync(request.EventId, cancellationToken);

        if (calendarEvent is null)
            return Result.Failure(new Error("event.not_found", "Event was not found."));

        var userId = _currentUser.UserId;


        if (!Enum.TryParse<EventParticipantStatus>(request.Status, true, out var status))
        {
            return Result.Failure(new Error("invitation.invalid_status", "Status must be accepted or declined."));
        }


        var result = status switch
        {
            EventParticipantStatus.Accepted => calendarEvent.AcceptInvitation(userId),
            EventParticipantStatus.Declined => calendarEvent.DeclineInvitation(userId),
            _ => DomainResult.Failure(new DomainError("invitation.invalid_status", "Status must be accepted or declined."))
        };

        if (result.IsFailure)
        {
            return result.Error!.ToApplicationFailure();
        }

        await _uow.BeginTransactionAsync(cancellationToken);

        try
        {
            await _uow.CalendarEvents.UpdateAsync(calendarEvent, cancellationToken);
            await _uow.CommitTransactionAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            await _uow.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}