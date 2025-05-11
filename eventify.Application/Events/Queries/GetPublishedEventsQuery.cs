using eventify.Application.Common;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Events.Queries;

public record GetPublishedEventsQuery(bool IncludeDeleted): IQuery<Result<IList<Event>>>;

public class GetPublishedEventsQueryHandler : IQueryHandler<GetPublishedEventsQuery, Result<IList<Event>>>
{
    private readonly IEventRepository _repository;

    public GetPublishedEventsQueryHandler(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IList<Event>>> Handle(GetPublishedEventsQuery request, CancellationToken cancellationToken)
    {
        if (request.IncludeDeleted)
        {
            return Result.Failure<IList<Event>>("IncludeDeleted is not supported.");
        }

        var events = await _repository.GetPublishedEventsAsync();
        return Result.Success((IList<Event>)events);
    }
}
