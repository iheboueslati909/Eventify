using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;

namespace eventify.Application.Repositories;

public interface IMemberRepository : IRepository<Member>
{
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Member>> GetActiveMembersAsync();
    Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Member>> GetMembersWithArtistProfilesAsync();
}