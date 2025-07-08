using Microsoft.AspNetCore.Mvc;
using eventify.Application.Tickets.Commands;
using eventify.Application.Common;
using eventify.SharedKernel;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;

    public TicketsController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketCommand command)
    {
        var result = await _commandDispatcher.Dispatch<CreateTicketCommand, Result<Guid>>(command, CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(null, new { id = result.Value }, result.Value);
    }
}
