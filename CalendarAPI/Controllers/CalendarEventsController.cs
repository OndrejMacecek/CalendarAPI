using CalendarAPI.Application.CalendarEvents.Commands;
using CalendarAPI.Contracts.Requests.CalendarEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CalendarAPI.Controllers;

[ApiController]
[Route("api/calendar-events")]
public sealed class CalendarEventsController : ControllerBase
{
    private readonly ISender _sender;

    public CalendarEventsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCalendarEventRequest request,
        CancellationToken cancellationToken)
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

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
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

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpDelete("{eventId:guid}")]
    public async Task<IActionResult> Delete(
    Guid eventId,
    CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteCalendarEventCommand(eventId), cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}