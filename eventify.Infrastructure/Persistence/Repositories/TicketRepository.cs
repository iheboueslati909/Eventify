using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;
using eventify.Infrastructure.Extensions;
using eventify.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eventify.Infrastructure.Persistence.Repositories;

public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
{
    public TicketRepository(EventsDbContext context) : base(context)
    {
    }

    public async Task<Ticket?> GetByIdWithEventAndCreatorAsync(Guid ticketId)
    {
        return await _context.Tickets
            .Include(t => t.Event)
            .Include(t => t.Creator)
            .FirstOrDefaultAsync(t => t.Id == ticketId);
    }
}
