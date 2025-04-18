using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;

namespace eventify.Application.Repositories;

public interface IArtistProfileRepository : IRepository<ArtistProfile>
{
    Task<IEnumerable<ArtistProfile>> GetActiveProfilesAsync();
    Task<IEnumerable<ArtistProfile>> GetProfilesByMemberIdAsync(Guid memberId);
    Task<bool> ArtistNameExistsAsync(string artistName);
}
