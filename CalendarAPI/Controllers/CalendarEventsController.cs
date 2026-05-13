using CalendarAPI.Application.CalendarEvents.Commands;
using CalendarAPI.Application.CalendarEvents.Queries;
using CalendarAPI.Contracts.Requests.CalendarEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CalendarAPI.Controllers;

[ApiController]
[Route("api/calendar-events")]
public sealed class CalendarEventsController 
    : ControllerBase
{
    private readonly ISender _sender;

    public CalendarEventsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCalendarEventRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCalendarEventCommand(
            request.CalendarId,
            request.EventTypeId,
            request.Title,
            request.Description,
            request.StartAtUtc,
            request.EndAtUtc,
            request.ParticipantIds);

        var result = await _sender.Send(command, cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(new { id = result.Value });
    }

    [HttpPut("{eventId:guid}")]
    public async Task<IActionResult> Update(
        Guid eventId, UpdateCalendarEventRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCalendarEventCommand(
            eventId,
            request.EventTypeId,
            request.Title,
            request.Description,
            request.StartAtUtc,
            request.EndAtUtc,
            request.ParticipantIds);

        var result = await _sender.Send(command, cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok();
    }

    [HttpDelete("{eventId:guid}")]
    public async Task<IActionResult> Delete(Guid eventId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteCalendarEventCommand(eventId), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result);
    }

    [HttpGet("{calendarId:guid}/calendar_events")]
    public async Task<IActionResult> GetCalendarEvents(
        Guid calendarId,
        [FromQuery(Name = "from_utc")] DateTimeOffset fromUtc,
        [FromQuery(Name = "to_utc")] DateTimeOffset toUtc,
    CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCalendarEventsQuery(calendarId, fromUtc, toUtc), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyEvents(
        [FromQuery(Name = "from_utc")] DateTimeOffset fromUtc,
        [FromQuery(Name = "to_utc")] DateTimeOffset toUtc,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMyEventsQuery(fromUtc, toUtc), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }
}