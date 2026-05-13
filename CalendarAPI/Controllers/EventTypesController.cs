using CalendarAPI.Application.EventTypes.Commands;
using CalendarAPI.Application.EventTypes.Queries;
using CalendarAPI.Contracts.Requests.EventTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CalendarAPI.Controllers;

[ApiController]
[Route("api/event-types")]
public class EventTypesController
    : ControllerBase
{
    private readonly ISender _sender;

    public EventTypesController(
        ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailable([FromQuery(Name = "calendar_id")] Guid calendarId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAvailableEventTypesQuery(calendarId), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEventTypeRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateEventTypeCommand(
            request.Name,
            request.Color,
            request.Priority,
            request.Scope,
            request.CalendarId);

        var result = await _sender.Send(command, cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(new { id = result.Value });
    }
}
