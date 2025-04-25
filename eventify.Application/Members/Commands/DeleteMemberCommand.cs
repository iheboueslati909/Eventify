using eventify.Application.Common.Interfaces;
using eventify.Application.Repositories;
using eventify.Application.Members.Queries;
using eventify.SharedKernel;

namespace eventify.Application.Members.Commands;

public record DeleteMemberCommand(Guid Id);

public class DeleteMemberCommandHandler
{
    private readonly IMemberRepository _memberRepository;

    public DeleteMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result> Handle(DeleteMemberCommand request)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id);
        if (member == null)
            return Result.Failure("Member not found");

        member.SoftDelete();
        await _memberRepository.SaveChangesAsync();
        
        return Result.Success();
    }
}
