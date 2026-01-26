using CoworkingManagement.Api.Models.Rooms;
using CoworkingManagement.Application.Features.Rooms.Commands.CreateRoomCommand;
using CoworkingManagement.Application.Features.Rooms.Commands.RemoveRoomCommand;
using CoworkingManagement.Application.Features.Rooms.Commands.UpdateRoomCommand;
using CoworkingManagement.Application.Features.Rooms.Queries.GetRoomById;
using CoworkingManagement.Application.Features.Rooms.Queries.GetRoomsList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingManagement.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RoomsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoomById(Guid id)
    {
        var getRoomByIdQueryResult = await _mediator.Send(new GetRoomByIdQuery(id));
        return Ok(getRoomByIdQueryResult);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRooms()
    {
        var getRoomListQueryResult = await _mediator.Send(new GetRoomsListQuery());
        return Ok(getRoomListQueryResult);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomCommand command)
    {
        var createRoomResult = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetRoomById), new { id = createRoomResult }, createRoomResult);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomRequest request)
    {
        var command = new UpdateRoomCommand(
            RoomId: id,
            Capacity: request.Capacity,
            Location: request.Location
        );
        await _mediator.Send(command);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new RemoveRoomCommand(
            RoomId: id
        );
        await _mediator.Send(command);

        return NoContent();
    }
}