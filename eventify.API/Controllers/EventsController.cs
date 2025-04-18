using Microsoft.AspNetCore.Mvc;
using eventify.Application.Common.Interfaces;
using eventify.Application.Events.Queries;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IEventRepository _repository;

    public EventsController(IEventRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("published")]
    public async Task<IActionResult> GetPublished()
    {
        var handler = new GetPublishedEventsQueryHandler(_repository);
        var result = await handler.Handle(new GetPublishedEventsQuery());

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
