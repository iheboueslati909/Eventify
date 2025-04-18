using Eventify.Domain.Members;
using eventify.Application.Common.Interfaces;

namespace eventify.Application.Repositories;

public interface IMemberFollowRepository : IRepository<MemberFollow>
{
    Task<IEnumerable<MemberFollow>> GetFollowsByMemberIdAsync(Guid memberId);
    Task<IEnumerable<MemberFollow>> GetFollowersByTargetIdAsync(Guid targetId);
    Task<bool> FollowExistsAsync(Guid memberId, Guid targetId);
}
