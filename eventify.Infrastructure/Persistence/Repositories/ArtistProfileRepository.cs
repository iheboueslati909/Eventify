using eventify.Application.Repositories;
using eventify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eventify.Infrastructure.Persistence.Repositories;

public class ArtistProfileRepository : BaseRepository<ArtistProfile>, IArtistProfileRepository
{
    public ArtistProfileRepository(EventsDbContext context) : base(context) { }

    public async Task<IEnumerable<ArtistProfile>> GetActiveProfilesAsync()
    {
        return await _context.Set<ArtistProfile>()
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<ArtistProfile>> GetProfilesByMemberIdAsync(Guid memberId)
    {
        return await _context.Set<ArtistProfile>()
            .Where(p => p.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<bool> ArtistNameExistsAsync(string artistName)
    {
        return await _context.Set<ArtistProfile>()
            .AnyAsync(p => p.ArtistName.Value == artistName);
    }
}
