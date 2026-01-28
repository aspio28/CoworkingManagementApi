using CoworkingManagement.Application.Features.Users.Commands.GetUserList;
using CoworkingManagement.Application.Features.Users.Commands.UpdateUserRole;
using CoworkingManagement.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingManagement.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet]
    public async Task<IActionResult>  GetUserList([FromQuery] GetUserListQuery query)
    {
        var getUserListQueryResult = await _mediator.Send(query);

        return Ok(getUserListQueryResult);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateUserRole(System.Guid id, [FromBody] UserRole role)
    {
        var command = new UpdateUserRoleCommand(id, role);
        
        await _mediator.Send(command);

        return NoContent();
    }
}