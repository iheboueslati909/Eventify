using eventify.Application.Common.Interfaces;
using eventify.Domain.ValueObjects;
using eventify.Domain.Common;
using eventify.SharedKernel;

namespace eventify.Application.Members.Commands;

public class UpdateMemberCommand
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UpdateMemberCommandHandler
{
    private readonly IMemberRepository _memberRepository;

    public UpdateMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result> Handle(UpdateMemberCommand request)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id);
        
        if (member == null)
            return Result.Failure("Member not found");

        try
        {
            member.UpdateInformation(
                new Name(request.FirstName),
                new Name(request.LastName),
                new Email(request.Email)
            );

            await _memberRepository.SaveChangesAsync();
            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
