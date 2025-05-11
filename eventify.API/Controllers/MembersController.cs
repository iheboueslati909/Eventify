using eventify.Application.Members.Commands;
using eventify.Application.Members.Queries;
using eventify.Application.Common;
using Microsoft.AspNetCore.Mvc;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public MembersController(
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
    {
            var result = await _queryDispatcher.Dispatch<GetMembersQuery, Result<IList<Member>>>(
        new GetMembersQuery(includeDeleted),
        CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _queryDispatcher.Dispatch<GetActiveMembersQuery, Result<IList<Member>>>(
            new GetActiveMembersQuery(),
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _queryDispatcher.Dispatch<GetMemberByIdQuery, Result<Member>>(
            new GetMemberByIdQuery(id),
            CancellationToken.None);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{id}/artist-profiles")]
    public async Task<IActionResult> GetArtistProfiles(Guid id)
    {
        var result = await _queryDispatcher.Dispatch<GetArtistProfilesByMemberQuery, Result<IList<ArtistProfile>>>(
            new GetArtistProfilesByMemberQuery(id),
            CancellationToken.None);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var result = await _queryDispatcher.Dispatch<GetMemberByEmailQuery, Result<Member>>(
            new GetMemberByEmailQuery(email),
            CancellationToken.None);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("email/exists/{email}")]
    public async Task<IActionResult> EmailExists(string email)
    {
        var result = await _queryDispatcher.Dispatch<EmailExistsQuery, Result<bool>>(
            new EmailExistsQuery(email),
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMemberCommand command)
    {
        var result = await _commandDispatcher.Dispatch<CreateMemberCommand, Result<Guid>>(
            command,
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMemberCommand command)
    {
        var result = await _commandDispatcher.Dispatch<UpdateMemberCommand, Result>(command with { Id = id });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _commandDispatcher.Dispatch<DeleteMemberCommand,Result>(new DeleteMemberCommand(id));

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
}
