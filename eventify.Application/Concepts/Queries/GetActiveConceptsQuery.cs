using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Concepts.Queries;

public record GetActiveConceptsQuery();

public class GetActiveConceptsQueryHandler
{
    private readonly IConceptRepository _repository;

    public GetActiveConceptsQueryHandler(IConceptRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Concept>>> Handle(GetActiveConceptsQuery request)
    {
        var concepts = await _repository.GetActiveConceptsAsync();
        return Result.Success(concepts);
    }
}
