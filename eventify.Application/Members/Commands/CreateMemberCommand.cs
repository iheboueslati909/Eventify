using eventify.Application.Common.Interfaces;
using eventify.Domain.Common;
using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;

namespace eventify.Application.Members.Commands;

public class CreateMemberCommand
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CreateMemberHandler
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<Guid>> Handle(CreateMemberCommand request)
    {
        if (await _memberRepository.EmailExistsAsync(request.Email))
            return Result.Failure<Guid>("Email already exists");

        try
        {
            var member = Member.Create(
                new Name(request.FirstName),
                new Name(request.LastName),
                new Email(request.Email),
                new Password(request.Password)
            );

            await _memberRepository.AddAsync(member);
            await _memberRepository.SaveChangesAsync();

            return Result.Success(member.Id);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<Guid>(ex.Message);
        }
    }
}
