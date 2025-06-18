using eventify.Application.Common;
using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;

namespace eventify.Application.Members.Commands;

public record CreateMemberCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Result<Guid>>;
    
public class CreateMemberHandler : ICommandHandler<CreateMemberCommand, Result<Guid>>
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Guid>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _memberRepository.EmailExistsAsync(request.Email);
        if (emailExists)
            return Result.Failure<Guid>("Email already exists");

        var firstNameResult = Name.Create(request.FirstName);
        var lastNameResult = Name.Create(request.LastName);
        var emailResult = Email.Create(request.Email);

        if (firstNameResult.IsFailure)
            return Result.Failure<Guid>(firstNameResult.Error);

        if (lastNameResult.IsFailure)
            return Result.Failure<Guid>(lastNameResult.Error);

        if (emailResult.IsFailure)
            return Result.Failure<Guid>(emailResult.Error);

        var member = Member.Create(
            firstNameResult.Value,
            lastNameResult.Value,
            emailResult.Value
        );

        if (member.IsFailure)
            return Result.Failure<Guid>(member.Error);

        await _memberRepository.AddAsync(member.Value);
        await _memberRepository.SaveChangesAsync();

        return Result.Success(member.Value.Id);
    }
}
