using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Members.Queries;

public record GetActiveMembersQuery();

public class GetActiveMembersQueryHandler
{
    private readonly IMemberRepository _memberRepository;

    public GetActiveMembersQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<IEnumerable<Member>>> Handle(GetActiveMembersQuery request)
    {
        var members = await _memberRepository.GetActiveMembersAsync();
        return Result.Success(members);
    }
}
