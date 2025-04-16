using eventify.Application.Common.Interfaces;
using MediatR;

namespace eventify.Application.Members.Commands;

public class DeleteMemberCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand>
{
    private readonly IMemberRepository _memberRepository;

    public DeleteMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Unit> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id);
        if (member == null)
        {
            throw new KeyNotFoundException("Member not found.");
        }

        _memberRepository.Remove(member);
        await _memberRepository.SaveChangesAsync();

        return Unit.Value;
    }
}
