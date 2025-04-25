using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;

namespace eventify.Application.Repositories;

public interface IMemberRepository : IRepository<Member>
{
    Task<IEnumerable<Member>> GetActiveMembersAsync();
    Task<IEnumerable<ArtistProfile>> GetArtistProfilesByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Member>> GetMembersWithArtistProfilesAsync();
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}