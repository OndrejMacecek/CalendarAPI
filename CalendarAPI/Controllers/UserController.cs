using CalendarAPI.Application.Users.Commands;
using CalendarAPI.Application.Users.Queries;
using CalendarAPI.Contracts.Requests.Users;
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
        var result = await _sender.Send(new CreateUserCommand(request.Email, request.DisplayName), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> Delete(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteUserCommand(userId), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserByIdQuery(id), cancellationToken);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUsersQuery(), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }
}
