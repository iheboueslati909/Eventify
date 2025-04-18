using Microsoft.AspNetCore.Mvc;
using eventify.Application.Repositories;
using eventify.Application.ArtistProfiles.Queries;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/artist-profiles")]
public class ArtistProfilesController : ControllerBase
{
    private readonly IArtistProfileRepository _repository;

    public ArtistProfilesController(IArtistProfileRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var handler = new GetActiveArtistProfilesQueryHandler(_repository);
        var result = await handler.Handle(new GetActiveArtistProfilesQuery());

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("member/{memberId}")]
    public async Task<IActionResult> GetByMemberId(Guid memberId)
    {
        var profiles = await _repository.GetProfilesByMemberIdAsync(memberId);
        return Ok(profiles);
    }
}
