using eventify.Application.Common.Interfaces;
using eventify.Application.Repositories;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;

namespace eventify.Application.Members.Commands;

public record UpdateMemberCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);

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

        var firstNameResult = Name.Create(request.FirstName);
        var lastNameResult = Name.Create(request.LastName);
        var emailResult = Email.Create(request.Email);

        if (firstNameResult.IsFailure)
            return Result.Failure(firstNameResult.Error);

        if (lastNameResult.IsFailure)
            return Result.Failure(lastNameResult.Error);

        if (emailResult.IsFailure)
            return Result.Failure(emailResult.Error);

        member.UpdateInformation(
            firstNameResult.Value,
            lastNameResult.Value,
            emailResult.Value
        );

        await _memberRepository.SaveChangesAsync();
        return Result.Success();
    }
}
