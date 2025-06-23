using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eventify.Infrastructure.Persistence.Repositories;

public class ArtistProfileRepository : BaseRepository<ArtistProfile>,IArtistProfileRepository
{

    public ArtistProfileRepository(EventsDbContext context): base(context)
    {
    }

    public async Task<IList<ArtistProfile>> GetByIdsAsync(IEnumerable<Guid> artistProfileIds, CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles
            .Where(ap => artistProfileIds.Contains(ap.Id))
            .ToListAsync(cancellationToken);
    }
}
