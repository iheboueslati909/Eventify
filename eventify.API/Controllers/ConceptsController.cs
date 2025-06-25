using Microsoft.AspNetCore.Mvc;
using eventify.Application.Common.Interfaces;
using eventify.Application.Concepts.Queries;
using eventify.Application.Concepts.Commands;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/concepts")]
public class ConceptsController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public ConceptsController(
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var result = await _queryDispatcher.Dispatch<GetActiveConceptsQuery, Result<IList<Concept>>>(
            new GetActiveConceptsQuery(),
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConceptCommand command)
    {
        var result = await _commandDispatcher.Dispatch<CreateConceptCommand, Result<Guid>>(
            command,
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetActive), new { id = result.Value }, result.Value);
    }
}
