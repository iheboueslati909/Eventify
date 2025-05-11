using eventify.Application.Common;
using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Members.Queries;

public record GetActiveMembersQuery(): IQuery<Result<IList<Member>>>;

public class GetActiveMembersQueryHandler  : IQueryHandler<GetActiveMembersQuery, Result<IList<Member>>>
{
    private readonly IMemberRepository _memberRepository;

    public GetActiveMembersQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<IList<Member>>> Handle(GetActiveMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _memberRepository.GetActiveMembersAsync();
        return Result.Success(members);
    }
}
