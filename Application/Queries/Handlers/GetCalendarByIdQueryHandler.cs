using CalendarAPI.Application.Calendars.Dtos;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;

namespace CalendarAPI.Application.Queries.Handlers;
public sealed class GetCalendarByIdQueryHandler
    : IQueryHandler<GetCalendarByIdQuery, CalendarDto>
{
    public async Task<Result<CalendarDto>> Handle(
        GetCalendarByIdQuery request,
        CancellationToken cancellationToken)
    {
        var dto = new CalendarDto(request.Id, "Demo");

        return Result<CalendarDto>.Success(dto);
    }
}
