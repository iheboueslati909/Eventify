using eventify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using eventify.Application.Repositories;

namespace eventify.Infrastructure.Persistence.Repositories;

public class MemberRepository : BaseRepository<Member>, IMemberRepository
{
    public MemberRepository(EventsDbContext context) : base(context) { }

    public async Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {

        return await _context.Members
            .FirstOrDefaultAsync(m => m.Email.Value == email, cancellationToken);
    }
    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .AsNoTracking()
            .AnyAsync(m => m.Email.Value == email, cancellationToken);
    }
    public async Task<IEnumerable<Member>> GetActiveMembersAsync()
    {
        return await _context.Members
            .Where(m => !m.IsDeleted)
            .ToListAsync();
    }
    public async Task<IEnumerable<Member>> GetMembersWithArtistProfilesAsync()
    {
        return await _context.Members
            .Include(m => m.ArtistProfiles)
            .Where(m => m.ArtistProfiles.Any())
            .ToListAsync();
    }
    public async Task<IEnumerable<ArtistProfile>> GetArtistProfilesByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles
            .Where(ap => ap.MemberId == memberId)
            .ToListAsync(cancellationToken);
    }
    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }
}
