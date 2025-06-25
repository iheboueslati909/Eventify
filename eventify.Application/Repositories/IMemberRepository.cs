using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;

namespace eventify.Application.Repositories;

public interface IMemberRepository : IRepository<Member>
{
    Task<IList<Member>> GetActiveMembersAsync();
    Task<IList<ArtistProfile>> GetArtistProfilesByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IList<Member>> GetMembersWithArtistProfilesAsync();
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<IList<Member>> GetByIdsAsync(IList<Guid> ids, CancellationToken cancellationToken = default);
}