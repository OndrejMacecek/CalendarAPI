using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;
using MediatR;

namespace CalendarAPI.Application.CalendarEvents.Commands.Handlers;
public sealed class DeleteCalendarEventCommandHandler
    : IRequestHandler<DeleteCalendarEventCommand, Result>
{
    private readonly ICalendarApiUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public DeleteCalendarEventCommandHandler(
        ICalendarApiUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        DeleteCalendarEventCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.UserId;

        if (currentUserId == Guid.Empty)
        {
            return Result<Guid>.Failure(new Error("auth.user_missing", "Missing or invalid user header."));
        }

        var calendarEvent = await _unitOfWork.CalendarEvents.GetByIdAsync(request.EventId, cancellationToken);

        if (calendarEvent is null)
        {
            return Result.Failure(new("calendar.not_found", "Calendar event not found."));
        }

        if (calendarEvent.OwnerUserId != currentUserId)
        {
            return Result.Failure(new("event.forbiden", "Only event owner can delete the event."));
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        await _unitOfWork.CalendarEvents.DeleteAsync(
            request.EventId,
            cancellationToken);

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return Result.Success();
    }
}