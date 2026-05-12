using CalendarAPI.Application.CalendarEvents.Commands;
using CalendarAPI.Application.CalendarEvents.Queries;
using CalendarAPI.Contracts.Requests.CalendarEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CalendarAPI.Controllers;
[ApiController]
[Route("api/invitations")]
public sealed class InvitationsController : ControllerBase
{
    private readonly ISender _sender;

    public InvitationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyInvitations(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMyInvitationsQuery(), cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPut("{eventId:guid}/participation")]
    public async Task<IActionResult> RespondToInvitation(Guid eventId, RespondToEventInvitationRequest request, CancellationToken cancellationToken)
    {
        var command = new RespondToEventInvitationCommand(eventId, request.Status);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

}