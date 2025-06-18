using eventify.Application.Auth.Commands;
using eventify.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using eventify.SharedKernel;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;

    public AuthController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _commandDispatcher.Dispatch<RegisterCommand, Result<Guid>>(
            command, 
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _commandDispatcher.Dispatch<LoginCommand, Result<string>>(
            command, 
            CancellationToken.None);

        if (result.IsFailure)
            return Unauthorized(result.Error);

        return Ok(new { token = result.Value });
    }
}
