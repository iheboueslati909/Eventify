using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;
using eventify.Infrastructure.Extensions;
using eventify.Application.Repositories;

namespace eventify.Infrastructure.Persistence.Repositories;

public class TicketPurchaseRepository : BaseRepository<TicketPurchase>, ITicketPurchaseRepository
{
    public TicketPurchaseRepository(EventsDbContext context) : base(context)
    {
    }

}
