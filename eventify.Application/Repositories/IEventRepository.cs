using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;

public interface IEventRepository : IRepository<Event>
{
    Task<IList<Event>> GetPublishedEventsAsync();
}