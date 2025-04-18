using Eventify.Domain.Members;
using eventify.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eventify.Infrastructure.Persistence.Repositories;

public class MemberFollowRepository : BaseRepository<MemberFollow>, IMemberFollowRepository
{
    public MemberFollowRepository(EventsDbContext context) : base(context) { }

    public async Task<IEnumerable<MemberFollow>> GetFollowsByMemberIdAsync(Guid memberId)
    {
        return await _context.Set<MemberFollow>()
            .Where(f => f.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MemberFollow>> GetFollowersByTargetIdAsync(Guid targetId)
    {
        return await _context.Set<MemberFollow>()
            .Where(f => f.TargetId == targetId)
            .ToListAsync();
    }

    public async Task<bool> FollowExistsAsync(Guid memberId, Guid targetId)
    {
        return await _context.Set<MemberFollow>()
            .AnyAsync(f => f.MemberId == memberId && f.TargetId == targetId);
    }
}
