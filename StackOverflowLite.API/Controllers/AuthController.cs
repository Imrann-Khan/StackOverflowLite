using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Features.Auth.Commands.Login;
using StackOverflowLite.Application.Features.Auth.Commands.Register;
using StackOverflowLite.Application.Features.Auth.Queries.GetProfile;

namespace StackOverflowLite.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ISender mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        return Ok(await mediator.Send(command));
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return Ok(await mediator.Send(new GetProfileQuery(userId)));
    }
}
