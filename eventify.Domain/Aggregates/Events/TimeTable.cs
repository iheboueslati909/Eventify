using eventify.Domain.ValueObjects;

namespace eventify.Domain.Entities;

public class TimeTable
{
    public Guid Id { get; private set; }
    public Title StageName { get; private set; }
    private readonly List<TimeTableSlot> _slots = new();
    public IReadOnlyCollection<TimeTableSlot> Slots => _slots.AsReadOnly();

    private TimeTable() { } // EF Core

    public TimeTable(Title stageName, List<TimeTableSlot> initialSlots)
    {
        if (initialSlots == null || !initialSlots.Any())
            throw new ArgumentException("TimeTable must have at least one slot");

        Id = Guid.NewGuid();
        StageName = stageName;
        _slots.AddRange(initialSlots);
    }

    public void UpdateTimeTableSlots(List<TimeTableSlot> newSlots)
    {
        if (newSlots == null || !newSlots.Any())
            throw new ArgumentException("TimeTable must have at least one slot");

        _slots.Clear();
        _slots.AddRange(newSlots);
    }
}