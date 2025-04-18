using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Members.Queries;

public record GetMemberByEmailQuery(string Email);

public class GetMemberByEmailQueryHandler
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByEmailQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(GetMemberByEmailQuery request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (member == null)
            return Result.Failure<Member>("Member not found");

        return Result.Success(member);
    }
}
