namespace eventify.Domain.Aggregates;
using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;

public class TimeTableAggregate
{
    public Guid Id { get; private set; }
    public Title StageName { get; private set; } // Value object for stage name
    private readonly List<TimeTableSlot> _slots = new(); // Manage slots directly
    public IReadOnlyCollection<TimeTableSlot> Slots => _slots.AsReadOnly();

    private TimeTableAggregate() { } // Required for EF Core

    public TimeTableAggregate(Title stageName)
    {
        StageName = stageName ?? throw new ArgumentNullException(nameof(stageName));
        Id = Guid.NewGuid();
    }

    public void AddSlot(TimeSpan startTime, TimeSpan endTime, Title title)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time.", nameof(startTime));

        var slot = new TimeTableSlot(Id, startTime, endTime, title);
        _slots.Add(slot);
    }

    public void RemoveSlot(Guid slotId)
    {
        var slot = _slots.FirstOrDefault(s => s.Id == slotId);
        if (slot == null)
            throw new InvalidOperationException("Slot not found.");

        _slots.Remove(slot);
    }

    public void UpdateStageName(Title newStageName)
    {
        StageName = newStageName ?? throw new ArgumentNullException(nameof(newStageName));
    }
}
