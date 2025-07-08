using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;

namespace eventify.Application.Repositories;

public interface ITicketRepository : IRepository<Ticket>
{
    //GetByIdWithEventAndCreatorAsync
    Task<Ticket?> GetByIdWithEventAndCreatorAsync(Guid ticketId);
}
