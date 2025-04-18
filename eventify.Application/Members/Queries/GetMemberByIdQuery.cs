using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Members.Queries;

public record GetMemberByIdQuery(Guid Id);

public class GetMemberByIdQueryHandler
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByIdQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Member>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id);
        if (member == null)
        {
            return Result.Failure<Member>($"Member with ID {request.Id} not found.");
        }

        return Result.Success(member);
    }
}
