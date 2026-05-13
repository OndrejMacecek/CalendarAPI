using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;
using MediatR;

namespace CalendarAPI.Application.Users.Commands.Handlers;
public sealed class DeleteUserCommandHandler
    : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly ICalendarApiUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public DeleteUserCommandHandler(
        ICalendarApiUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.UserId;

        if (request.UserId != currentUserId)
            return Result.Failure(new("user.forbidden", "User can delete only himself."));

        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(new("user.not_fount", "User not found."));
        }

        await _unitOfWork.Users.DeleteUserGraphAsync(request.UserId, cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return Result.Success();
    }
}