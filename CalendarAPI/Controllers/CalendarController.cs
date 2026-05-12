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
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("{calendarId:guid}/calendar_events")]
    public async Task<IActionResult> GetCalendarEvents(
        Guid calendarId,
        [FromQuery(Name = "from_utc")] DateTimeOffset fromUtc,
        [FromQuery(Name = "to_utc")] DateTimeOffset toUtc,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetCalendarEventsQuery(calendarId, fromUtc, toUtc),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyEvents(
        [FromQuery(Name = "from_utc")] DateTimeOffset fromUtc,
        [FromQuery(Name = "to_utc")] DateTimeOffset toUtc,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMyEventsQuery(fromUtc, toUtc), cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
