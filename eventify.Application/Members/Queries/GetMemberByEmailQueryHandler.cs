using eventify.Application.Common;
using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Members.Queries;

public record GetMemberByEmailQuery(string Email) : IQuery<Result<Member>>;

public class GetMemberByEmailQueryHandler : IQueryHandler<GetMemberByEmailQuery, Result<Member>>
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
            return Result.Failure<Member>($"Member with email {request.Email} not found.");

        return Result.Success(member);
    }
}