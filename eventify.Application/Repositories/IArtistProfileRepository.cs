using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;

namespace eventify.Application.Repositories;

public interface IArtistProfileRepository  : IRepository<ArtistProfile>
{
    Task<IList<ArtistProfile>> GetByIdsAsync(IEnumerable<Guid> artistProfileIds, CancellationToken cancellationToken = default);
}
