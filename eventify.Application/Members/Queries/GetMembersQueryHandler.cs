using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.SharedKernel;
using eventify.Application.Common;

namespace eventify.Application.Members.Queries;

public record GetMembersQuery(bool IncludeDeleted) : IQuery<Result<IList<Member>>>;

public class GetMembersQueryHandler  : IQueryHandler<GetMembersQuery, Result<IList<Member>>>
{
    private readonly IMemberRepository _memberRepository;

    public GetMembersQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<IList<Member>>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //todo : implement concelation token
                var members = await _memberRepository.GetAllAsync(request.IncludeDeleted);
                
                if (!request.IncludeDeleted)
                {
                    members = members.Where(m => !m.IsDeleted).ToList();
                }

                return Result.Success(members);
            }
            catch (Exception ex)
            {
                return Result.Failure<IList<Member>>(ex.Message);
            }
        }
}
