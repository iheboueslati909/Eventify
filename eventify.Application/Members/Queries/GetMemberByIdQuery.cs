using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using MediatR;

namespace eventify.Application.Members.Queries;

public class GetMemberByIdQuery : IRequest<Member>
{
    public Guid Id { get; set; }
}

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, Member>
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByIdQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Member> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id);
        if (member == null)
        {
            throw new KeyNotFoundException("Member not found.");
        }

        return member;
    }
}
