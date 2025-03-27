using eventify.Domain.Entities;

namespace eventify.Domain.Aggregates;

public class TimeTableSlotAggregate
{
    public TimeTableSlot TimeTableSlot { get; private set; }
    public RecordedPerformance? RecordedPerformance { get; private set; }

    private TimeTableSlotAggregate() { } // Required for EF Core

    public TimeTableSlotAggregate(TimeTableSlot slot)
    {
        TimeTableSlot = slot;
    }

    public void AddRecordedPerformance(RecordedPerformance recordedPerformance)
    {
        RecordedPerformance = recordedPerformance;
    }
}
