//Get artist profiles by member query
using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Members.Queries;

public record GetArtistProfilesByMemberQuery(Guid MemberId);

public class GetArtistProfilesByMemberQueryHandler
{
    private readonly IMemberRepository _memberRepository;

    public GetArtistProfilesByMemberQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<IEnumerable<ArtistProfile>>> Handle(GetArtistProfilesByMemberQuery request, CancellationToken cancellationToken)
    {
        var artistProfiles = await _memberRepository.GetArtistProfilesByMemberIdAsync(request.MemberId, cancellationToken);
        return Result.Success(artistProfiles);
    }
}