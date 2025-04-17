using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using eventify.Domain.Common;
using eventify.SharedKernel;

namespace eventify.Application.Members.Queries;

public class GetMemberQuery
{
    public Guid Id { get; set; }
}

public class GetMemberQueryHandler
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(GetMemberQuery request)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id);
        
        if (member == null)
            return Result.Failure<Member>("Member not found");

        return Result.Success(member);
    }
}
