using eventify.Domain.Aggregates;
using System.Threading.Tasks;

namespace eventify.Domain.Interfaces;

public interface IMemberRepository
{
    Task AddAsync(MemberAggregate memberAggregate);
    Task<MemberAggregate?> GetByIdAsync(int memberId);
    Task UpdateAsync(MemberAggregate memberAggregate);
    Task DeleteAsync(int memberId);
}
