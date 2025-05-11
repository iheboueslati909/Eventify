using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eventify.Infrastructure.Persistence.Repositories;

public class ConceptRepository : BaseRepository<Concept>, IConceptRepository
{
    public ConceptRepository(EventsDbContext context) : base(context) { }
    public async Task<IList<Concept>> GetActiveConceptsAsync()
    {
        return await _context.Set<Concept>().Where(c => !c.IsDeleted).ToListAsync();
    }
}
