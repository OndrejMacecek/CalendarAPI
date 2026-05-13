using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;
using CalendarAPI.Domain.Calendars.Aggregates;
using CalendarAPI.Domain.Users.Aggregates;

namespace CalendarAPI.Application.Users.Commands.Handlers;
public sealed class CreateUserCommandHandler
    : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly ICalendarApiUnitOfWork _uow;

    public CreateUserCommandHandler(
        ICalendarApiUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _uow.Users.GetByEmailAsync(request.email, cancellationToken);

        if (existingUser is not null)
        {
            return Result<Guid>.Failure(
                new Error("user.email_already_exists", "User with this email already exists."));
        }

        var userResult = User.Create(request.email, request.displayName);

        if (userResult.IsFailure)
        {
            return userResult.Error!.ToApplicationFailure<Guid>();
        }

        var user = userResult.Value!;

        var calendarResult = Calendar.Create(user.Id, "Default calendar");

        if (calendarResult.IsFailure)
        {
            return calendarResult.Error!.ToApplicationFailure<Guid>();
        }

        await _uow.BeginTransactionAsync(cancellationToken);

        try
        {
            await _uow.Users.AddAsync(user, cancellationToken);
            await _uow.Calendars.AddAsync(calendarResult.Value!, cancellationToken);

            await _uow.CommitTransactionAsync(cancellationToken);

            return Result<Guid>.Success(user.Id);
        }
        catch
        {
            await _uow.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}