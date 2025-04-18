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

    public MembersController(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false)
    {
        var handler = new GetMembersQueryHandler(_memberRepository);
        var result = await handler.Handle(new GetMembersQuery { IncludeDeleted = includeDeleted });
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var handler = new GetMemberQueryHandler(_memberRepository);
        var result = await handler.Handle(new GetMemberQuery { Id = id });

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var handler = new GetMemberByEmailQueryHandler(_memberRepository);
        var result = await handler.Handle(new GetMemberByEmailQuery(email), default);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMemberCommand command)
    {
        var handler = new CreateMemberHandler(_memberRepository);
        var result = await handler.Handle(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMemberCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var handler = new UpdateMemberCommandHandler(_memberRepository);
        var result = await handler.Handle(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var handler = new DeleteMemberCommandHandler(_memberRepository);
        var result = await handler.Handle(new DeleteMemberCommand(id));

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
}
