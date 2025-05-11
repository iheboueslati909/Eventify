//Get artist profiles by member query
using eventify.Application.Common;
using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Members.Queries;

public record GetArtistProfilesByMemberQuery(Guid MemberId) : IQuery<Result<IList<ArtistProfile>>>;

public class GetArtistProfilesByMemberQueryHandler : IQueryHandler<GetArtistProfilesByMemberQuery, Result<IList<ArtistProfile>>>
{
    private readonly IMemberRepository _memberRepository;

    public GetArtistProfilesByMemberQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<IList<ArtistProfile>>> Handle(GetArtistProfilesByMemberQuery request, CancellationToken cancellationToken)
    {
        var artistProfiles = await _memberRepository.GetArtistProfilesByMemberIdAsync(request.MemberId, cancellationToken);
        return Result.Success(artistProfiles);
    }
}