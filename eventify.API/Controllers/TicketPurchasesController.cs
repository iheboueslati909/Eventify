using Microsoft.AspNetCore.Mvc;
using eventify.Application.Tickets.Commands;
using eventify.Application.Common;
using eventify.SharedKernel;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/ticket-purchases")]
public class TicketPurchasesController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;

    public TicketPurchasesController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketPurchaseCommand command)
    {
        var result = await _commandDispatcher.Dispatch<CreateTicketPurchaseCommand, Result<CreateTicketPurchaseResult>>(command, CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        // Return the full result (including checkoutUrl and paymentId)
        return CreatedAtAction(null, new { id = result.Value.TicketPurchaseId }, result.Value);
    }

    [HttpPost("{id}/fulfill")]
    public async Task<IActionResult> Fulfill(Guid id, [FromBody] FullFillTicketPurchaseCommand fulfillCommand)
    {
        // Ensure the command uses the correct TicketPurchaseId from the route
        var command = fulfillCommand with { TicketPurchaseId = id };
        var result = await _commandDispatcher.Dispatch<FullFillTicketPurchaseCommand, Result>(command, CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }
}
