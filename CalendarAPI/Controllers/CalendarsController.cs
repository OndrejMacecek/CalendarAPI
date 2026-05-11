using CalendarAPI.Application.Calendars.Dtos;
using CalendarAPI.Application.Commands;
using CalendarAPI.Contracts.Requests.Calendars;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CalendarAPI.Controllers;


[ApiController]
[Route("api/calendars")]
public sealed class CalendarsController : ControllerBase
{
    private readonly ISender _sender;

    public CalendarsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCalendarRequest request,
        CancellationToken cancellationToken)
    {
        var appRequest = new CalendarDto(Guid.Empty, request.Name);
        var command = new CreateCalendarCommand(appRequest);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}