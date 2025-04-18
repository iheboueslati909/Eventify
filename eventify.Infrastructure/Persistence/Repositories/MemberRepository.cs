using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
       

}
