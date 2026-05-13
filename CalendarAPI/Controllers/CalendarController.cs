using CalendarAPI.Application.CalendarEvents.Queries;
using CalendarAPI.Application.Calendars.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CalendarAPI.Controllers;

[ApiController]
[Route("api/calendars")]
public class CalendarController
    : ControllerBase
{
    private readonly ISender _sender;

    public CalendarController(
        ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{calendarId:guid}")]
    public async Task<IActionResult> GetById(Guid calendarId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCalendarByIdQuery(calendarId));
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }
}
