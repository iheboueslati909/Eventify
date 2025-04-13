using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eventify.Infrastructure.Persistence.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(EventsDbContext context) : base(context) { }

    public async Task<IEnumerable<Event>> GetPublishedEventsAsync()
    {
        return await _context.Set<Event>().Where(e => e.IsPublished && !e.IsDeleted).ToListAsync();
    }
}
