using Microsoft.AspNetCore.Mvc;
using eventify.Application.Common.Interfaces;
using eventify.Application.Concepts.Queries;

namespace eventify.API.Controllers;

[ApiController]
[Route("api/concepts")]
public class ConceptsController : ControllerBase
{
    private readonly IConceptRepository _repository;

    public ConceptsController(IConceptRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var handler = new GetActiveConceptsQueryHandler(_repository);
        var result = await handler.Handle(new GetActiveConceptsQuery());

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
