using CoworkingManagement.Application.Features.Users.Commands.Login;
using CoworkingManagement.Application.Features.Users.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoworkingManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Register([FromBody] LoginCommand command)
    {
        var authResult = await _mediator.Send(command);

        return Ok(authResult);
    }
}