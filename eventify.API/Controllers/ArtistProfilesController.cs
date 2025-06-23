using Microsoft.AspNetCore.Mvc;
using eventify.Application.Common;
using eventify.Application.Members.Commands;
using eventify.SharedKernel;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/artist-profiles")]
public class ArtistProfilesController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;

    public ArtistProfilesController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArtistProfileCommand command)
    {
        var result = await _commandDispatcher.Dispatch<CreateArtistProfileCommand, Result<Guid>>(
            command,
            CancellationToken.None);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(null, new { id = result.Value }, result.Value);
    }
}
