using eventify.Application.Members.Commands;
using eventify.Application.Members.Queries;
using Microsoft.AspNetCore.Mvc;
using eventify.Application.Repositories;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IMemberRepository _memberRepository;
    private readonly GetMembersQueryHandler _getMembersHandler;
    private readonly GetActiveMembersQueryHandler _getActiveMembersHandler;
    private readonly GetMemberByIdQueryHandler _getMemberByIdHandler;
    private readonly GetArtistProfilesByMemberQueryHandler _getArtistProfilesHandler;
    private readonly GetMemberByEmailQueryHandler _getMemberByEmailHandler;
    private readonly EmailExistsQueryHandler _emailExistsHandler;
    private readonly CreateMemberHandler _createMemberHandler;
    private readonly UpdateMemberCommandHandler _updateMemberHandler;
    private readonly DeleteMemberCommandHandler _deleteMemberHandler;

    public MembersController(
        IMemberRepository memberRepository,
        GetMembersQueryHandler getMembersHandler,
        GetActiveMembersQueryHandler getActiveMembersHandler,
        GetMemberByIdQueryHandler getMemberByIdHandler,
        GetArtistProfilesByMemberQueryHandler getArtistProfilesHandler,
        GetMemberByEmailQueryHandler getMemberByEmailHandler,
        EmailExistsQueryHandler emailExistsHandler,
        CreateMemberHandler createMemberHandler,
        UpdateMemberCommandHandler updateMemberHandler,
        DeleteMemberCommandHandler deleteMemberHandler)
    {
        _memberRepository = memberRepository;
        _getMembersHandler = getMembersHandler;
        _getActiveMembersHandler = getActiveMembersHandler;
        _getMemberByIdHandler = getMemberByIdHandler;
        _getArtistProfilesHandler = getArtistProfilesHandler;
        _getMemberByEmailHandler = getMemberByEmailHandler;
        _emailExistsHandler = emailExistsHandler;
        _createMemberHandler = createMemberHandler;
        _updateMemberHandler = updateMemberHandler;
        _deleteMemberHandler = deleteMemberHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
    {
        var result = await _getMembersHandler.Handle(new GetMembersQuery { IncludeDeleted = includeDeleted });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _getActiveMembersHandler.Handle(new GetActiveMembersQuery());

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getMemberByIdHandler.Handle(new GetMemberByIdQuery(id), default);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{id}/artist-profiles")]
    public async Task<IActionResult> GetArtistProfiles(Guid id)
    {
        var result = await _getArtistProfilesHandler.Handle(new GetArtistProfilesByMemberQuery(id), default);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var result = await _getMemberByEmailHandler.Handle(new GetMemberByEmailQuery(email), default);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("email/exists/{email}")]
    public async Task<IActionResult> EmailExists(string email)
    {
        var result = await _emailExistsHandler.Handle(new EmailExistsQuery(email), default);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMemberCommand command)
    {
        var result = await _createMemberHandler.Handle(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMemberCommand command)
    {
        var result = await _updateMemberHandler.Handle(command with { Id = id });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _deleteMemberHandler.Handle(new DeleteMemberCommand(id));

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
}
