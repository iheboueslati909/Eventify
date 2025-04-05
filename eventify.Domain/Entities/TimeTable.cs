namespace eventify.Domain.Entities;

public class TimeTable
{
    public Guid Id { get; private set; }
    public string StageName { get; private set; }
    private readonly List<TimeTableSlot> _slots = new();

    public IReadOnlyCollection<TimeTableSlot> Slots => _slots.AsReadOnly();

    private TimeTable() { } // Required for EF Core

    public TimeTable(string stageName)
    {
        if (string.IsNullOrWhiteSpace(stageName))
            throw new ArgumentException("Stage name cannot be empty.", nameof(stageName));

        Id = Guid.NewGuid();
        StageName = stageName;
    }

    public void AddSlot(TimeTableSlot slot)
    {
        if (slot == null) throw new ArgumentNullException(nameof(slot));
        _slots.Add(slot);
    }

    public void RemoveSlot(Guid slotId)
    {
        var slot = _slots.FirstOrDefault(s => s.Id == slotId);
        if (slot == null)
            throw new InvalidOperationException("Slot not found.");

        _slots.Remove(slot);
    }
}
