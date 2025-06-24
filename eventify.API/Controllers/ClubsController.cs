using Microsoft.AspNetCore.Mvc;
using eventify.Application.Clubs.Commands;
using eventify.Application.Clubs.Queries;
using eventify.Application.Common;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/clubs")]
public class ClubsController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public ClubsController(
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
    {
        var result = await _queryDispatcher.Dispatch<GetClubsQuery, Result<IList<Club>>>(
            new GetClubsQuery(includeDeleted),
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClubCommand command)
    {
        var result = await _commandDispatcher.Dispatch<CreateClubCommand, Result<Guid>>(
            command,
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetAll), new { id = result.Value }, result.Value);
    }
}
