using CalendarAPI.Application.Calendars.UnitOfWorks;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Domain.Calendars.Entities;

namespace CalendarAPI.Application.Commands.Handlers;

public sealed class CreateCalendarCommandHandler
    : ICommandHandler<CreateCalendarCommand, Guid>
{
    private readonly ICalendarUnitOfWork _calendarUow;

    public CreateCalendarCommandHandler(
        ICalendarUnitOfWork calendarUow)
    {
        _calendarUow = calendarUow;
    }
    public async Task<Result<Guid>> Handle(CreateCalendarCommand request, CancellationToken cancellationToken)
    {
        var calendarResult = Calendar.Create(request.Calendar.Name);

        if (calendarResult.IsFailure)
            return Result<Guid>.Failure(calendarResult.Error!);

        var calendar = calendarResult.Value!;

        await _calendarUow.BeginTransactionAsync(cancellationToken);

        try
        {
            await _calendarUow.Calendars.AddAsync(calendar, cancellationToken);
            await _calendarUow.CommitTransactionAsync(cancellationToken);

            var data = await _calendarUow.Calendars.GetByIdAsync(calendar.Id, cancellationToken);
            var datas = await _calendarUow.Calendars.GetAllAsync(cancellationToken);
            return Result<Guid>.Success(calendar.Id);
        }
        catch
        {
            await _calendarUow.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
