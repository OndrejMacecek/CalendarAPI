using CalendarAPI.Application.Users.Commands;
using CalendarAPI.Application.Users.Queries;
using CalendarAPI.Contracts.Requests.Users;
using CalendarAPI.Contracts.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CalendarAPI.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UserController
    : ControllerBase
{
    private readonly ISender _sender;

    public UserController(
        ISender sender
        )
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(request.Email, request.DisplayName);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new CreateUserResponse { Id = result.Value });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUsersQuery(), cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("my_calendars")]
    public async Task<IActionResult> GetMyCalendars(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMyCalendarsQuery(), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> Delete(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteUserCommand(userId), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok();
    }

}
