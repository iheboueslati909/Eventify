using eventify.Domain.Aggregates;
using System.Threading.Tasks;

namespace eventify.Domain.Interfaces;

public interface ITimeTableSlotRepository
{
    Task AddAsync(TimeTableSlotAggregate timeTableSlotAggregate);
    Task<TimeTableSlotAggregate?> GetByIdAsync(int timeTableSlotId);
    Task UpdateAsync(TimeTableSlotAggregate timeTableSlotAggregate);
    Task DeleteAsync(int timeTableSlotId);
}
