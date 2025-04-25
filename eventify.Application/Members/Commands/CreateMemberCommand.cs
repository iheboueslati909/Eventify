using eventify.Application.Common.Interfaces;
using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;
using eventify.Application.Members.Queries;

namespace eventify.Application.Members.Commands;

public record CreateMemberCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password);
    
public class CreateMemberHandler
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Guid>> Handle(CreateMemberCommand request)
    {
        var emailExists = await _memberRepository.EmailExistsAsync(request.Email);
        if (emailExists)
            return Result.Failure<Guid>("Email already exists");

        var firstNameResult = Name.Create(request.FirstName);
        var lastNameResult = Name.Create(request.LastName);
        var emailResult = Email.Create(request.Email);
        var passwordResult = Password.Create(request.Password);

        if (firstNameResult.IsFailure)
            return Result.Failure<Guid>(firstNameResult.Error);

        if (lastNameResult.IsFailure)
            return Result.Failure<Guid>(lastNameResult.Error);

        if (emailResult.IsFailure)
            return Result.Failure<Guid>(emailResult.Error);

        if (passwordResult.IsFailure)
            return Result.Failure<Guid>(passwordResult.Error);

        var member = Member.Create(
            firstNameResult.Value,
            lastNameResult.Value,
            emailResult.Value,
            passwordResult.Value
        );

        if (member.IsFailure)
            return Result.Failure<Guid>(member.Error);

        await _memberRepository.AddAsync(member.Value);
        await _memberRepository.SaveChangesAsync();

        return Result.Success(member.Value.Id);
    }
}
