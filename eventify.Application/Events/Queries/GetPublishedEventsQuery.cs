using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Events.Queries;

public record GetPublishedEventsQuery();

public class GetPublishedEventsQueryHandler
{
    private readonly IEventRepository _repository;

    public GetPublishedEventsQueryHandler(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Event>>> Handle(GetPublishedEventsQuery request)
    {
        var events = await _repository.GetPublishedEventsAsync();
        return Result.Success(events);
    }
}
