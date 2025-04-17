using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using eventify.Domain.Common;
using eventify.SharedKernel;

namespace eventify.Application.Members.Queries;

public class GetMembersQuery
{
    public bool IncludeDeleted { get; set; } = false;
}

public class GetMembersQueryHandler
{
    private readonly IMemberRepository _memberRepository;

    public GetMembersQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<IEnumerable<Member>>> Handle(GetMembersQuery request)
    {
        try
        {
            var members = await _memberRepository.GetAllAsync();
            
            if (!request.IncludeDeleted)
                members = members.Where(m => !m.IsDeleted);

            return Result.Success(members);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<Member>>(ex.Message);
        }
    }
}
