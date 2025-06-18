using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using eventify.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eventify.Infrastructure.Persistence.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(EventsDbContext context) : base(context) { }

    public async Task<IList<Event>> GetPublishedEventsAsync()
    {
        return await _context.Set<Event>().Where(e => e.IsPublished && !e.IsDeleted).ToListAsync();
    }
}
