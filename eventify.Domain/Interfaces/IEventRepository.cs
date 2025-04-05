using eventify.Domain.Aggregates;
using System.Threading.Tasks;

namespace eventify.Domain.Interfaces;

public interface IEventRepository
{
    Task AddAsync(EventAggregate eventAggregate);
    Task<EventAggregate?> GetByIdAsync(int eventId);
    Task UpdateAsync(EventAggregate eventAggregate);
    Task DeleteAsync(int eventId);
}
