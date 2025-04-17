using eventify.Application.Members.Commands;
using eventify.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMemberCommand command)
    {
        var handler = new CreateMemberHandler(_memberRepository);

        var memberId = await handler.Handle(command);

        return CreatedAtAction(nameof(GetById), new { id = memberId }, new { id = memberId });
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        // Optional placeholder for now
        return Ok(new { id });
    }
}
