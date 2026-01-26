using CoworkingManagement.Api.Models.Reservations;
using CoworkingManagement.Application.Features.Reservations.Commands.CancelReservation;
using CoworkingManagement.Application.Features.Reservations.Commands.CreateReservation;
using CoworkingManagement.Application.Features.Reservations.Commands.UpdateReservation;
using CoworkingManagement.Application.Features.Reservations.Queries.GetReservationById;
using CoworkingManagement.Application.Features.Reservations.Queries.GetReservationsList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingManagement.Api.Controllers;
// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReservationsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var getReservationByIdQueryResult = await _mediator.Send(new GetReservationByIdQuery(id));
        return Ok(getReservationByIdQueryResult);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReservations([FromQuery] GetReservationsListQuery query)
    {
        var getAllReservationsQueryResult = await _mediator.Send(query);
        return Ok(getAllReservationsQueryResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationCommand command)
    {
        var createReservationResult = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = createReservationResult }, createReservationResult);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReservationRequest request)
    {
        var command = new UpdateReservationCommand(
            ReservationId: id,
            RoomId: request.RoomId,
            StartDate: request.StartDate,
            EndDate: request.EndDate
        );

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var command = new CancelReservationCommand(ReservationId: id);

        await _mediator.Send(command);

        return NoContent();
    }
}