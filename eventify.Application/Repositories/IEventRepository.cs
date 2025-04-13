using eventify.Domain.Entities;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetPublishedEventsAsync();
}