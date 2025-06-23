using Microsoft.AspNetCore.Mvc;
using eventify.Application.Common.Interfaces;
using eventify.Application.Events.Queries;
using eventify.Application.Events.Commands;
using eventify.SharedKernel;
using eventify.Domain.Entities;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public EventsController(
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet("published")]
    public async Task<IActionResult> GetPublished([FromQuery] bool includeDeleted = false)
    {
        var result = await _queryDispatcher.Dispatch<GetPublishedEventsQuery, Result<IList<Event>>>(
            new GetPublishedEventsQuery(includeDeleted),
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventCommand command)
    {
        var result = await _commandDispatcher.Dispatch<CreateEventCommand, Result<Guid>>(
            command,
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        // Return 201 Created with the event ID in the response body
        return CreatedAtAction(nameof(GetPublished), new { id = result.Value }, result.Value);
    }
}
