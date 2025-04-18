using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.ArtistProfiles.Queries;

public record GetActiveArtistProfilesQuery();

public class GetActiveArtistProfilesQueryHandler
{
    private readonly IArtistProfileRepository _repository;

    public GetActiveArtistProfilesQueryHandler(IArtistProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ArtistProfile>>> Handle(GetActiveArtistProfilesQuery request)
    {
        var profiles = await _repository.GetActiveProfilesAsync();
        return Result.Success(profiles);
    }
}
