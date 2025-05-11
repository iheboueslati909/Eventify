//Email exists Query
using eventify.Application.Common;
using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.SharedKernel;

namespace eventify.Application.Members.Queries;

public record EmailExistsQuery(string Email) : IQuery<Result<bool>>;

public class EmailExistsQueryHandler : IQueryHandler<EmailExistsQuery, Result<bool>>
{
    private readonly IMemberRepository _memberRepository;

    public EmailExistsQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<bool>> Handle(EmailExistsQuery request, CancellationToken cancellationToken)
    {
        var exists = await _memberRepository.EmailExistsAsync(request.Email, cancellationToken);
        return Result.Success(exists);
    }
}