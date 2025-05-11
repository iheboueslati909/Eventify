using eventify.Application.Common;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Concepts.Queries;

public record GetActiveConceptsQuery() : IQuery<Result<IList<Concept>>>;

public class GetActiveConceptsQueryHandler : IQueryHandler<GetActiveConceptsQuery, Result<IList<Concept>>>
{
    private readonly IConceptRepository _repository;

    public GetActiveConceptsQueryHandler(IConceptRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IList<Concept>>> Handle(GetActiveConceptsQuery request, CancellationToken cancellationToken)
    {
        var concepts = await _repository.GetActiveConceptsAsync();
        return Result.Success(concepts);
    }
}
