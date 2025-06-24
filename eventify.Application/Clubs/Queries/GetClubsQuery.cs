using eventify.Application.Common;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Clubs.Queries;

public record GetClubsQuery(bool IncludeDeleted) : IQuery<Result<IList<Club>>>;

public class GetClubsQueryHandler : IQueryHandler<GetClubsQuery, Result<IList<Club>>>
{
    private readonly IClubRepository _clubRepository;

    public GetClubsQueryHandler(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }

    public async Task<Result<IList<Club>>> Handle(GetClubsQuery request, CancellationToken cancellationToken)
    {
        var clubs = await _clubRepository.GetAllAsync(request.IncludeDeleted);
        if (!request.IncludeDeleted)
        {
            clubs = clubs.Where(c => !c.IsDeleted).ToList();
        }
        return Result.Success(clubs);
    }
}
