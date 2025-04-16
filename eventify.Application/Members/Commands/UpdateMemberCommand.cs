using eventify.Application.Common.Interfaces;
using eventify.Domain.ValueObjects;
using MediatR;

namespace eventify.Application.Members.Commands;

public class UpdateMemberCommand : IRequest
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand>
{
    private readonly IMemberRepository _memberRepository;

    public UpdateMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Unit> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id);
        if (member == null)
        {
            throw new KeyNotFoundException("Member not found.");
        }

        member.UpdateInformation(
            new Name(request.FirstName),
            new Name(request.LastName),
            new Email(request.Email)
        );

        _memberRepository.Update(member);
        await _memberRepository.SaveChangesAsync();

        return Unit.Value;
    }
}
