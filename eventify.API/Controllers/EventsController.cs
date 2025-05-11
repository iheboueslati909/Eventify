using Microsoft.AspNetCore.Mvc;
using eventify.Application.Common.Interfaces;
using eventify.Application.Events.Queries;
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
}
