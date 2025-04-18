using eventify.Application.Repositories;
using Eventify.Domain.Members;
using eventify.SharedKernel;

namespace eventify.Application.MemberFollows.Queries;

public record GetFollowersByTargetQuery(Guid TargetId);

public class GetFollowersByTargetQueryHandler
{
    private readonly IMemberFollowRepository _repository;

    public GetFollowersByTargetQueryHandler(IMemberFollowRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<MemberFollow>>> Handle(GetFollowersByTargetQuery request)
    {
        var follows = await _repository.GetFollowersByTargetIdAsync(request.TargetId);
        return Result.Success(follows);
    }
}
