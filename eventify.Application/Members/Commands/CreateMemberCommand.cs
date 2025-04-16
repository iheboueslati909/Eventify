using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using MediatR;

namespace eventify.Application.Members.Commands;

public class CreateMemberCommand : IRequest<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Guid>
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Guid> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = Member.Create(
            new Name(request.FirstName),
            new Name(request.LastName),
            new Email(request.Email),
            new Password(request.Password)
        );

        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync();

        return member.Id;
    }
}
